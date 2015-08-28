
#!/usr/bin/perl

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# dungeon.pl
#
# Random Dungeon Generator by drow
# http://donjon.bin.sh/
#
# This code is provided under the
# Creative Commons Attribution-NonCommercial 3.0 Unported License
# http://creativecommons.org/licenses/by-nc/3.0/

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# use perl

use strict;
use lib '/usr/local/lib/perl5';
use GD;
  GD::Image->trueColor(1);

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# configuration

my $dungeon_layout = {
  'Box'         => [[1,1,1],[1,0,1],[1,1,1]],
  'Cross'       => [[0,1,0],[1,1,1],[0,1,0]],
};
my $corridor_layout = {
  'Labyrinth'   =>   0,
  'Bent'        =>  50,
  'Straight'    => 100,
};
my $map_style = {
  'Standard' => {
    'fill'      => '000000',
    'open'      => 'FFFFFF',
    'open_grid' => 'CCCCCC',
  },
};

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# cell bits

my $NOTHING     = 0x00000000;

my $BLOCKED     = 0x00000001;
my $ROOM        = 0x00000002;
my $CORRIDOR    = 0x00000004;
#                 0x00000008;
my $PERIMETER   = 0x00000010;
my $ENTRANCE    = 0x00000020;
my $ROOM_ID     = 0x0000FFC0;

my $ARCH        = 0x00010000;
my $DOOR        = 0x00020000;
my $LOCKED      = 0x00040000;
my $TRAPPED     = 0x00080000;
my $SECRET      = 0x00100000;
my $PORTC       = 0x00200000;
my $STAIR_DN    = 0x00400000;
my $STAIR_UP    = 0x00800000;

my $LABEL       = 0xFF000000;

my $OPENSPACE   = $ROOM | $CORRIDOR;
my $DOORSPACE   = $ARCH | $DOOR | $LOCKED | $TRAPPED | $SECRET | $PORTC;
my $ESPACE      = $ENTRANCE | $DOORSPACE | 0xFF000000;
my $STAIRS      = $STAIR_DN | $STAIR_UP;

my $BLOCK_ROOM  = $BLOCKED | $ROOM;
my $BLOCK_CORR  = $BLOCKED | $PERIMETER | $CORRIDOR;
my $BLOCK_DOOR  = $BLOCKED | $DOORSPACE;

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# directions

my $di = { 'north' => -1, 'south' =>  1, 'west' =>  0, 'east' =>  0 };
my $dj = { 'north' =>  0, 'south' =>  0, 'west' => -1, 'east' =>  1 };
my @dj_dirs = sort keys %{ $dj };

my $opposite = {
  'north'       => 'south',
  'south'       => 'north',
  'west'        => 'east',
  'east'        => 'west'
};

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# stairs

my $stair_end = {
  'north' => {
    'walled'    => [[1,-1],[0,-1],[-1,-1],[-1,0],[-1,1],[0,1],[1,1]],
    'corridor'  => [[0,0],[1,0],[2,0]],
    'stair'     => [0,0],
    'next'      => [1,0],
  },
  'south' => {
    'walled'    => [[-1,-1],[0,-1],[1,-1],[1,0],[1,1],[0,1],[-1,1]],
    'corridor'  => [[0,0],[-1,0],[-2,0]],
    'stair'     => [0,0],
    'next'      => [-1,0],
  },
  'west' => {
    'walled'    => [[-1,1],[-1,0],[-1,-1],[0,-1],[1,-1],[1,0],[1,1]],
    'corridor'  => [[0,0],[0,1],[0,2]],
    'stair'     => [0,0],
    'next'      => [0,1],
  },
  'east' => {
    'walled'    => [[-1,-1],[-1,0],[-1,1],[0,1],[1,1],[1,0],[1,-1]],
    'corridor'  => [[0,0],[0,-1],[0,-2]],
    'stair'     => [0,0],
    'next'      => [0,-1],
  },
};

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# cleaning

my $close_end = {
  'north' => {
    'walled'    => [[0,-1],[1,-1],[1,0],[1,1],[0,1]],
    'close'     => [[0,0]],
    'recurse'   => [-1,0],
  },
  'south' => {
    'walled'    => [[0,-1],[-1,-1],[-1,0],[-1,1],[0,1]],
    'close'     => [[0,0]],
    'recurse'   => [1,0],
  },
  'west' => {
    'walled'    => [[-1,0],[-1,1],[0,1],[1,1],[1,0]],
    'close'     => [[0,0]],
    'recurse'   => [0,-1],
  },
  'east' => {
    'walled'    => [[-1,0],[-1,-1],[0,-1],[1,-1],[1,0]],
    'close'     => [[0,0]],
    'recurse'   => [0,1],
  },
};

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# imaging

my $color_chain = {
  'door'        => 'fill',
  'label'       => 'fill',
  'stair'       => 'wall',
  'wall'        => 'fill',
  'fill'        => 'black',
};

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# showtime

my $opts = &get_opts();
my $dungeon = &create_dungeon($opts);
   &image_dungeon($dungeon);

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# get dungeon options

sub get_opts {
  my $opts = {
    'seed'              => time(),
    'n_rows'            => 39,          # must be an odd number
    'n_cols'            => 39,          # must be an odd number
    'dungeon_layout'    => 'None',
    'room_min'          => 3,           # minimum room size
    'room_max'          => 9,           # maximum room size
    'room_layout'       => 'Packed', # Packed, Scattered
    'corridor_layout'   => 'Bent',
    'remove_deadends'   => 100,          # percentage
    'add_stairs'        => 2,           # number of stairs
    'map_style'         => 'Standard',
    'cell_size'         => 18,          # pixels
  };
  return $opts;
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# create dungeon

sub create_dungeon {
  my ($dungeon) = @_;

  $dungeon->{'n_i'} = int($dungeon->{'n_rows'} / 2);
  $dungeon->{'n_j'} = int($dungeon->{'n_cols'} / 2);
  $dungeon->{'n_rows'} = $dungeon->{'n_i'} * 2;
  $dungeon->{'n_cols'} = $dungeon->{'n_j'} * 2;
  $dungeon->{'max_row'} = $dungeon->{'n_rows'} - 1;
  $dungeon->{'max_col'} = $dungeon->{'n_cols'} - 1;
  $dungeon->{'n_rooms'} = 0;

  my $max = $dungeon->{'room_max'};
  my $min = $dungeon->{'room_min'};
  $dungeon->{'room_base'} = int(($min + 1) / 2);
  $dungeon->{'room_radix'} = int(($max - $min) / 2) + 1;

  $dungeon = &init_cells($dungeon);
  $dungeon = &emplace_rooms($dungeon);
  $dungeon = &open_rooms($dungeon);
  $dungeon = &label_rooms($dungeon);
  $dungeon = &corridors($dungeon);
  $dungeon = &emplace_stairs($dungeon) if ($dungeon->{'add_stairs'});
  $dungeon = &clean_dungeon($dungeon);

  return $dungeon;
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# initialize cells

sub init_cells {
  my ($dungeon) = @_;

  my $r; for ($r = 0; $r <= $dungeon->{'n_rows'}; $r++) {
    my $c; for ($c = 0; $c <= $dungeon->{'n_cols'}; $c++) {
      $dungeon->{'cell'}[$r][$c] = $NOTHING;
    }
  }
  srand($dungeon->{'seed'} + 0);

  my $mask; if ($mask = $dungeon_layout->{$dungeon->{'dungeon_layout'}}) {
    $dungeon = &mask_cells($dungeon,$mask);
  } elsif ($dungeon->{'dungeon_layout'} eq 'Round') {
    $dungeon = &round_mask($dungeon);
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# mask cells

# sub mask_cells {
#   my ($dungeon,$mask) = @_;
#   my $r_x = (scalar @{ $mask } * 1.0 / ($dungeon->{'n_rows'} + 1));
#   my $c_x = (scalar @{ $mask->[0] } * 1.0 / ($dungeon->{'n_cols'} + 1));
#   my $cell = $dungeon->{'cell'};

#   my $r; for ($r = 0; $r <= $dungeon->{'n_rows'}; $r++) {
#     my $c; for ($c = 0; $c <= $dungeon->{'n_cols'}; $c++) {
#       $cell->[$r][$c] = $BLOCKED unless ($mask->[$r * $r_x][$c * $c_x]);
#     }
#   }
#   return $dungeon;
# }

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# round mask

# sub round_mask {
#   my ($dungeon) = @_;
#   my $center_r = int($dungeon->{'n_rows'} / 2);
#   my $center_c = int($dungeon->{'n_cols'} / 2);
#   my $cell = $dungeon->{'cell'};

#   my $r; for ($r = 0; $r <= $dungeon->{'n_rows'}; $r++) {
#     my $c; for ($c = 0; $c <= $dungeon->{'n_cols'}; $c++) {
#       my $d = sqrt((($r - $center_r) ** 2) + (($c - $center_c) ** 2));
#       $cell->[$r][$c] = $BLOCKED if ($d > $center_c);
#     }
#   }
#   return $dungeon;
# }

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# emplace rooms

# sub emplace_rooms {
#   my ($dungeon) = @_;

#   if ($dungeon->{'room_layout'} eq 'Packed') {
#     $dungeon = &pack_rooms($dungeon);
#   } else {
#     $dungeon = &scatter_rooms($dungeon);
#   }
#   return $dungeon;
# }

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# pack rooms

sub pack_rooms {
  my ($dungeon) = @_;
  my $cell = $dungeon->{'cell'};

  my $i; for ($i = 0; $i < $dungeon->{'n_i'}; $i++) {
      my $r = ($i * 2) + 1;
    my $j; for ($j = 0; $j < $dungeon->{'n_j'}; $j++) {
      my $c = ($j * 2) + 1;

      next if ($cell->[$r][$c] & $ROOM);
      next if (($i == 0 || $j == 0) && int(rand(2)));

      my $proto = { 'i' => $i, 'j' => $j };
      $dungeon = &emplace_room($dungeon,$proto);
    }
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# scatter rooms

# sub scatter_rooms {
#   my ($dungeon) = @_;
#   my $n_rooms = &alloc_rooms($dungeon);

#   my $i; for ($i = 0; $i < $n_rooms; $i++) {
#     $dungeon = &emplace_room($dungeon);
#   }
#   return $dungeon;
# }

# # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# # allocate number of rooms

# sub alloc_rooms {
#   my ($dungeon) = @_;
#   my $dungeon_area = $dungeon->{'n_cols'} * $dungeon->{'n_rows'};
#   my $room_area = $dungeon->{'room_max'} * $dungeon->{'room_max'};
#   my $n_rooms = int($dungeon_area / $room_area);

#   return $n_rooms;
# }

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# emplace room

sub emplace_room {
  my ($dungeon,$proto) = @_;
     return $dungeon if ($dungeon->{'n_rooms'} == 999);
  my ($r,$c);
  my $cell = $dungeon->{'cell'};

  # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  # room position and size

  $proto = &set_room($dungeon,$proto);

  # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  # room boundaries

  my $r1 = ( $proto->{'i'}                       * 2) + 1;
  my $c1 = ( $proto->{'j'}                       * 2) + 1;
  my $r2 = (($proto->{'i'} + $proto->{'height'}) * 2) - 1;
  my $c2 = (($proto->{'j'} + $proto->{'width'} ) * 2) - 1;

  return $dungeon if ($r1 < 1 || $r2 > $dungeon->{'max_row'});
  return $dungeon if ($c1 < 1 || $c2 > $dungeon->{'max_col'});

  # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  # check for collisions with existing rooms

  my $hit = &sound_room($dungeon,$r1,$c1,$r2,$c2);
     return $dungeon if ($hit->{'blocked'});
  my @hit_list = keys %{ $hit };
  my $n_hits = scalar @hit_list;
  my $room_id;

  if ($n_hits == 0) {
    $room_id = $dungeon->{'n_rooms'} + 1;
    $dungeon->{'n_rooms'} = $room_id;
  } else {
    return $dungeon;
  }
  $dungeon->{'last_room_id'} = $room_id;

  # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  # emplace room

  for ($r = $r1; $r <= $r2; $r++) {
    for ($c = $c1; $c <= $c2; $c++) {
      if ($cell->[$r][$c] & $ENTRANCE) {
        $cell->[$r][$c] &= ~ $ESPACE;
      } elsif ($cell->[$r][$c] & $PERIMETER) {
        $cell->[$r][$c] &= ~ $PERIMETER;
      }
      $cell->[$r][$c] |= $ROOM | ($room_id << 6);
    }
  }
  my $height = (($r2 - $r1) + 1) * 10;
  my $width = (($c2 - $c1) + 1) * 10;

  my $room_data = {
    'id' => $room_id, 'row' => $r1, 'col' => $c1,
    'north' => $r1, 'south' => $r2, 'west' => $c1, 'east' => $c2,
    'height' => $height, 'width' => $width, 'area' => ($height * $width)
  };
  $dungeon->{'room'}[$room_id] = $room_data;

  # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  # block corridors from room boundary
  # check for door openings from adjacent rooms

  for ($r = $r1 - 1; $r <= $r2 + 1; $r++) {
    unless ($cell->[$r][$c1 - 1] & ($ROOM | $ENTRANCE)) {
      $cell->[$r][$c1 - 1] |= $PERIMETER;
    }
    unless ($cell->[$r][$c2 + 1] & ($ROOM | $ENTRANCE)) {
      $cell->[$r][$c2 + 1] |= $PERIMETER;
    }
  }
  for ($c = $c1 - 1; $c <= $c2 + 1; $c++) {
    unless ($cell->[$r1 - 1][$c] & ($ROOM | $ENTRANCE)) {
      $cell->[$r1 - 1][$c] |= $PERIMETER;
    }
    unless ($cell->[$r2 + 1][$c] & ($ROOM | $ENTRANCE)) {
      $cell->[$r2 + 1][$c] |= $PERIMETER;
    }
  }

  # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# room position and size

sub set_room {
  my ($dungeon,$proto) = @_;
  my $base = $dungeon->{'room_base'};
  my $radix = $dungeon->{'room_radix'};

  unless (defined $proto->{'height'}) {
    if (defined $proto->{'i'}) {
      my $a = $dungeon->{'n_i'} - $base - $proto->{'i'};
         $a = 0 if ($a < 0);
      my $r = ($a < $radix) ? $a : $radix;

      $proto->{'height'} = int(rand($r)) + $base;
    } else {
      $proto->{'height'} = int(rand($radix)) + $base;
    }
  }
  unless (defined $proto->{'width'}) {
    if (defined $proto->{'j'}) {
      my $a = $dungeon->{'n_j'} - $base - $proto->{'j'};
         $a = 0 if ($a < 0);
      my $r = ($a < $radix) ? $a : $radix;

      $proto->{'width'} = int(rand($r)) + $base;
    } else {
      $proto->{'width'} = int(rand($radix)) + $base;
    }
  }
  unless (defined $proto->{'i'}) {
    $proto->{'i'} = int(rand($dungeon->{'n_i'} - $proto->{'height'}));
  }
  unless (defined $proto->{'j'}) {
    $proto->{'j'} = int(rand($dungeon->{'n_j'} - $proto->{'width'}));
  }
  return $proto;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# sound room

sub sound_room {
  my ($dungeon,$r1,$c1,$r2,$c2) = @_;
  my $cell = $dungeon->{'cell'};
  my $hit;

  my $r; for ($r = $r1; $r <= $r2; $r++) {
    my $c; for ($c = $c1; $c <= $c2; $c++) {
      if ($cell->[$r][$c] & $BLOCKED) {
        return { 'blocked' => 1 };
      }
      if ($cell->[$r][$c] & $ROOM) {
        my $id = ($cell->[$r][$c] & $ROOM_ID) >> 6;
        $hit->{$id} += 1;
      }
    }
  }
  return $hit;
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# emplace openings for doors and corridors

sub open_rooms {
  my ($dungeon) = @_;

  my $id; for ($id = 1; $id <= $dungeon->{'n_rooms'}; $id++) {
    $dungeon = &open_room($dungeon,$dungeon->{'room'}[$id]);
  }
  delete($dungeon->{'connect'});
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# emplace openings for doors and corridors

sub open_room {
  my ($dungeon,$room) = @_;
  my @list = &door_sills($dungeon,$room);
     return $dungeon unless (@list);
  my $n_opens = &alloc_opens($dungeon,$room);
  my $cell = $dungeon->{'cell'};

  my $i; for ($i = 0; $i < $n_opens; $i++) {
    my $sill = splice(@list,int(rand(@list)),1);
       last unless ($sill);
    my $door_r = $sill->{'door_r'};
    my $door_c = $sill->{'door_c'};
    my $door_cell = $cell->[$door_r][$door_c];
       redo if ($door_cell & $DOORSPACE);

    my $out_id; if ($out_id = $sill->{'out_id'}) {
      my $connect = join(',',(sort($room->{'id'},$out_id)));
      redo if ($dungeon->{'connect'}{$connect}++);
    }
    my $open_r = $sill->{'sill_r'};
    my $open_c = $sill->{'sill_c'};
    my $open_dir = $sill->{'dir'};

    # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    # open door

    my $x; for ($x = 0; $x < 3; $x++) {
      my $r = $open_r + ($di->{$open_dir} * $x);
      my $c = $open_c + ($dj->{$open_dir} * $x);

      $cell->[$r][$c] &= ~ $PERIMETER;
      $cell->[$r][$c] |= $ENTRANCE;
    }
    my $door_type = &door_type();
    my $door = { 'row' => $door_r, 'col' => $door_c };

    if ($door_type == $ARCH) {
      $cell->[$door_r][$door_c] |= $ARCH;
      $door->{'key'} = 'arch'; $door->{'type'} = 'Archway';
    } elsif ($door_type == $DOOR) {
      $cell->[$door_r][$door_c] |= $DOOR;
      $cell->[$door_r][$door_c] |= (ord('o') << 24);
      $door->{'key'} = 'open'; $door->{'type'} = 'Unlocked Door';
    } elsif ($door_type == $LOCKED) {
      $cell->[$door_r][$door_c] |= $LOCKED;
      $cell->[$door_r][$door_c] |= (ord('x') << 24);
      $door->{'key'} = 'lock'; $door->{'type'} = 'Locked Door';
    } elsif ($door_type == $TRAPPED) {
      $cell->[$door_r][$door_c] |= $TRAPPED;
      $cell->[$door_r][$door_c] |= (ord('t') << 24);
      $door->{'key'} = 'trap'; $door->{'type'} = 'Trapped Door';
    } elsif ($door_type == $SECRET) {
      $cell->[$door_r][$door_c] |= $SECRET;
      $cell->[$door_r][$door_c] |= (ord('s') << 24);
      $door->{'key'} = 'secret'; $door->{'type'} = 'Secret Door';
    } elsif ($door_type == $PORTC) {
      $cell->[$door_r][$door_c] |= $PORTC;
      $cell->[$door_r][$door_c] |= (ord('#') << 24);
      $door->{'key'} = 'portc'; $door->{'type'} = 'Portcullis';
    }
    $door->{'out_id'} = $out_id if ($out_id);
    push(@{ $room->{'door'}{$open_dir} },$door) if ($door);
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# allocate number of opens

sub alloc_opens {
  my ($dungeon,$room) = @_;
  my $room_h = (($room->{'south'} - $room->{'north'}) / 2) + 1;
  my $room_w = (($room->{'east'} - $room->{'west'}) / 2) + 1;
  my $flumph = int(sqrt($room_w * $room_h));
  my $n_opens = $flumph + int(rand($flumph));

  return $n_opens;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# list available sills

sub door_sills {
  my ($dungeon,$room) = @_;
  my $cell = $dungeon->{'cell'};
  my @list;

  if ($room->{'north'} >= 3) {
    my $c; for ($c = $room->{'west'}; $c <= $room->{'east'}; $c += 2) {
      my $sill = &check_sill($cell,$room,$room->{'north'},$c,'north');
      push(@list,$sill) if ($sill);
    }
  }
  if ($room->{'south'} <= ($dungeon->{'n_rows'} - 3)) {
    my $c; for ($c = $room->{'west'}; $c <= $room->{'east'}; $c += 2) {
      my $sill = &check_sill($cell,$room,$room->{'south'},$c,'south');
      push(@list,$sill) if ($sill);
    }
  }
  if ($room->{'west'} >= 3) {
    my $r; for ($r = $room->{'north'}; $r <= $room->{'south'}; $r += 2) {
      my $sill = &check_sill($cell,$room,$r,$room->{'west'},'west');
      push(@list,$sill) if ($sill);
    }
  }
  if ($room->{'east'} <= ($dungeon->{'n_cols'} - 3)) {
    my $r; for ($r = $room->{'north'}; $r <= $room->{'south'}; $r += 2) {
      my $sill = &check_sill($cell,$room,$r,$room->{'east'},'east');
      push(@list,$sill) if ($sill);
    }
  }
  return &shuffle(@list);
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# check sill

sub check_sill {
  my ($cell,$room,$sill_r,$sill_c,$dir) = @_;
  my $door_r = $sill_r + $di->{$dir};
  my $door_c = $sill_c + $dj->{$dir};
  my $door_cell = $cell->[$door_r][$door_c];
     return unless ($door_cell & $PERIMETER);
     return if ($door_cell & $BLOCK_DOOR);
  my $out_r  = $door_r + $di->{$dir};
  my $out_c  = $door_c + $dj->{$dir};
  my $out_cell = $cell->[$out_r][$out_c];

  my $out_id; if ($out_cell & $ROOM) {
    $out_id = ($out_cell & $ROOM_ID) >> 6;
    return if ($out_id == $room->{'id'});
  }
  return {
    'sill_r'    => $sill_r,
    'sill_c'    => $sill_c,
    'dir'       => $dir,
    'door_r'    => $door_r,
    'door_c'    => $door_c,
    'out_id'    => $out_id,
  };
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# shuffle list

sub shuffle {
  my (@list) = @_;
  my @i = (0..((scalar @list) - 1));
  my $d = { map { $_ => int(rand(100)) } @i };
     @i = sort { $d->{$a} <=> $d->{$b} } @i;

  return @list[@i];
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# random door type

sub door_type {
  my $i = int(rand(110));

  if ($i < 15) {
    return $ARCH;
  } elsif ($i < 60) {
    return $DOOR;
  } elsif ($i < 75) {
    return $LOCKED;
  } elsif ($i < 90) {
    return $TRAPPED;
  } elsif ($i < 100) {
    return $SECRET;
  } else {
    return $PORTC;
  }
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# label rooms

sub label_rooms {
  my ($dungeon) = @_;
  my $cell = $dungeon->{'cell'};

  my $id; for ($id = 1; $id <= $dungeon->{'n_rooms'}; $id++) {
    my $room = $dungeon->{'room'}[$id];
    my $label = "$room->{'id'}";
    my $len = length($label);
    my $label_r = int(($room->{'north'} + $room->{'south'}) / 2);
    my $label_c = int(($room->{'west'} + $room->{'east'} - $len) / 2) + 1;

    my $c; for ($c = 0; $c < $len; $c++) {
      my $char = substr($label,$c,1);
      $cell->[$label_r][$label_c + $c] |= (ord($char) << 24);
    }
  }
  return $dungeon;
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# generate corridors

sub corridors {
  my ($dungeon) = @_;
  my $cell = $dungeon->{'cell'};

  my $i; for ($i = 1; $i < $dungeon->{'n_i'}; $i++) {
      my $r = ($i * 2) + 1;
    my $j; for ($j = 1; $j < $dungeon->{'n_j'}; $j++) {
      my $c = ($j * 2) + 1;

      next if ($cell->[$r][$c] & $CORRIDOR);
      $dungeon = &tunnel($dungeon,$i,$j);
    }
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# recursively tunnel

sub tunnel {
  my ($dungeon,$i,$j,$last_dir) = @_;
  my @dirs = &tunnel_dirs($dungeon,$last_dir);

  my $dir; foreach $dir (@dirs) {
    if (&open_tunnel($dungeon,$i,$j,$dir)) {
      my $next_i = $i + $di->{$dir};
      my $next_j = $j + $dj->{$dir};

      $dungeon = &tunnel($dungeon,$next_i,$next_j,$dir);
    }
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# tunnel directions

sub tunnel_dirs {
  my ($dungeon,$last_dir) = @_;
  my $p = $corridor_layout->{$dungeon->{'corridor_layout'}};
  my @dirs = &shuffle(@dj_dirs);

  if ($last_dir && $p) {
    unshift(@dirs,$last_dir) if (int(rand(100)) < $p);
  }
  return @dirs;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# open tunnel

sub open_tunnel {
  my ($dungeon,$i,$j,$dir) = @_;
  my $this_r = ($i * 2) + 1;
  my $this_c = ($j * 2) + 1;
  my $next_r = (($i + $di->{$dir}) * 2) + 1;
  my $next_c = (($j + $dj->{$dir}) * 2) + 1;
  my $mid_r = ($this_r + $next_r) / 2;
  my $mid_c = ($this_c + $next_c) / 2;

  if (&sound_tunnel($dungeon,$mid_r,$mid_c,$next_r,$next_c)) {
    return &delve_tunnel($dungeon,$this_r,$this_c,$next_r,$next_c);
  } else {
    return 0;
  }
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# sound tunnel
# don't open blocked cells, room perimeters, or other corridors

sub sound_tunnel {
  my ($dungeon,$mid_r,$mid_c,$next_r,$next_c) = @_;
     return 0 if ($next_r < 0 || $next_r > $dungeon->{'n_rows'});
     return 0 if ($next_c < 0 || $next_c > $dungeon->{'n_cols'});
  my $cell = $dungeon->{'cell'};
  my ($r1,$r2) = sort { $a <=> $b } ($mid_r,$next_r);
  my ($c1,$c2) = sort { $a <=> $b } ($mid_c,$next_c);

  my $r; for ($r = $r1; $r <= $r2; $r++) {
    my $c; for ($c = $c1; $c <= $c2; $c++) {
      return 0 if ($cell->[$r][$c] & $BLOCK_CORR);
    }
  }
  return 1;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# delve tunnel

sub delve_tunnel {
  my ($dungeon,$this_r,$this_c,$next_r,$next_c) = @_;
  my $cell = $dungeon->{'cell'};
  my ($r1,$r2) = sort { $a <=> $b } ($this_r,$next_r);
  my ($c1,$c2) = sort { $a <=> $b } ($this_c,$next_c);

  my $r; for ($r = $r1; $r <= $r2; $r++) {
    my $c; for ($c = $c1; $c <= $c2; $c++) {
      $cell->[$r][$c] &= ~ $ENTRANCE;
      $cell->[$r][$c] |= $CORRIDOR;
    }
  }
  return 1;
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# emplace stairs

sub emplace_stairs {
  my ($dungeon) = @_;
  my $n = $dungeon->{'add_stairs'};
     return $dungeon unless ($n > 0);
  my @list = &stair_ends($dungeon);
     return $dungeon unless (@list);
  my $cell = $dungeon->{'cell'};

  my $i; for ($i = 0; $i < $n; $i++) {
    my $stair = splice(@list,int(rand(@list)),1);
       last unless ($stair);
    my $r = $stair->{'row'};
    my $c = $stair->{'col'};
    my $type = ($i < 2) ? $i : int(rand(2));

    if ($type == 0) {
      $cell->[$r][$c] |= $STAIR_DN;
      $cell->[$r][$c] |= (ord('d') << 24);
      $stair->{'key'} = 'down';
    } else {
      $cell->[$r][$c] |= $STAIR_UP;
      $cell->[$r][$c] |= (ord('u') << 24);
      $stair->{'key'} = 'up';
    }
    push(@{ $dungeon->{'stair'} },$stair);
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# list available ends

sub stair_ends {
  my ($dungeon) = @_;
  my $cell = $dungeon->{'cell'};
  my @list;

  my $i; ROW: for ($i = 0; $i < $dungeon->{'n_i'}; $i++) {
      my $r = ($i * 2) + 1;
    my $j; COL: for ($j = 0; $j < $dungeon->{'n_j'}; $j++) {
      my $c = ($j * 2) + 1;

      next unless ($cell->[$r][$c] == $CORRIDOR);
      next if ($cell->[$r][$c] & $STAIRS);

      my $dir; foreach $dir (keys %{ $stair_end }) {
        if (&check_tunnel($cell,$r,$c,$stair_end->{$dir})) {
          my $end = { 'row' => $r, 'col' => $c };
          my $n = $stair_end->{$dir}{'next'};
             $end->{'next_row'} = $end->{'row'} + $n->[0];
             $end->{'next_col'} = $end->{'col'} + $n->[1];

          push(@list,$end); next COL;
        }
      }
    }
  }
  return @list;
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
# final clean-up

sub clean_dungeon {
  my ($dungeon) = @_;

  if ($dungeon->{'remove_deadends'}) {
    $dungeon = &remove_deadends($dungeon);
  }
  $dungeon = &fix_doors($dungeon);
  $dungeon = &empty_blocks($dungeon);

  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# remove deadend corridors

sub remove_deadends {
  my ($dungeon) = @_;
  my $p = $dungeon->{'remove_deadends'};

  return &collapse_tunnels($dungeon,$p,$close_end);
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# collapse tunnels

sub collapse_tunnels {
  my ($dungeon,$p,$xc) = @_;
     return $dungeon unless ($p);
  my $all = ($p == 100);
  my $cell = $dungeon->{'cell'};

  my $i; for ($i = 0; $i < $dungeon->{'n_i'}; $i++) {
      my $r = ($i * 2) + 1;
    my $j; for ($j = 0; $j < $dungeon->{'n_j'}; $j++) {
      my $c = ($j * 2) + 1;

      next unless ($cell->[$r][$c] & $OPENSPACE);
      next if ($cell->[$r][$c] & $STAIRS);
      next unless ($all || (int(rand(100)) < $p));

      $dungeon = &collapse($dungeon,$r,$c,$xc);
    }
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# collapse

sub collapse {
  my ($dungeon,$r,$c,$xc) = @_;
  my $cell = $dungeon->{'cell'};

  unless ($cell->[$r][$c] & $OPENSPACE) {
    return $dungeon;
  }
  my $dir; foreach $dir (keys %{ $xc }) {
    if (&check_tunnel($cell,$r,$c,$xc->{$dir})) {
      my $p; foreach $p (@{ $xc->{$dir}{'close'} }) {
        $cell->[$r+$p->[0]][$c+$p->[1]] = $NOTHING;
      }
      if ($p = $xc->{$dir}{'open'}) {
        $cell->[$r+$p->[0]][$c+$p->[1]] |= $CORRIDOR;
      }
      if ($p = $xc->{$dir}{'recurse'}) {
        $dungeon = &collapse($dungeon,($r+$p->[0]),($c+$p->[1]),$xc);
      }
    }
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# check tunnel

sub check_tunnel {
  my ($cell,$r,$c,$check) = @_;
  my $list;

  if ($list = $check->{'corridor'}) {
    my $p; foreach $p (@{ $list }) {
      return 0 unless ($cell->[$r+$p->[0]][$c+$p->[1]] == $CORRIDOR);
    }
  }
  if ($list = $check->{'walled'}) {
    my $p; foreach $p (@{ $list }) {
      return 0 if ($cell->[$r+$p->[0]][$c+$p->[1]] & $OPENSPACE);
    }
  }
  return 1;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# fix door lists

sub fix_doors {
  my ($dungeon) = @_;
  my $cell = $dungeon->{'cell'};
  my $fixed;

  my $room; foreach $room (@{ $dungeon->{'room'} }) {
    my $dir; foreach $dir (sort keys %{ $room->{'door'} }) {
      my ($door,@shiny); foreach $door (@{ $room->{'door'}{$dir} }) {
        my $door_r = $door->{'row'};
        my $door_c = $door->{'col'};
        my $door_cell = $cell->[$door_r][$door_c];
           next unless ($door_cell & $OPENSPACE);

        if ($fixed->[$door_r][$door_c]) {
          push(@shiny,$door);
        } else {
          my $out_id; if ($out_id = $door->{'out_id'}) {
            my $out_dir = $opposite->{$dir};
            push(@{ $dungeon->{'room'}[$out_id]{'door'}{$out_dir} },$door);
          }
          push(@shiny,$door);
          $fixed->[$door_r][$door_c] = 1;
        }
      }
      if (@shiny) {
        $room->{'door'}{$dir} = \@shiny;
        push(@{ $dungeon->{'door'} },@shiny);
      } else {
        delete $room->{'door'}{$dir};
      }
    }
  }
  return $dungeon;
}

# - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
# empty blocks

sub empty_blocks {
  my ($dungeon) = @_;
  my $cell = $dungeon->{'cell'};

  my $r; for ($r = 0; $r <= $dungeon->{'n_rows'}; $r++) {
    my $c; for ($c = 0; $c <= $dungeon->{'n_cols'}; $c++) {
      $cell->[$r][$c] = $NOTHING if ($cell->[$r][$c] & $BLOCKED);
    }
  }
  return $dungeon;
}

# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

