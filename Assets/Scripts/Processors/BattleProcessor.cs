using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleProcessor {

  Simulation sim;
  int iterationCount = 0;
  int iterationLimit = 20;

  Mob currentMob {
    get {
      return sim.currentMob;
    }
  }

  public BattleProcessor (Simulation _sim) {
    sim = _sim;
  }

  public void StartBattle (Mob mob) {

    sim.currentMob = mob;
    sim.player.lastBattleMove = null;
    sim.player.initiative = 0f;
    mob.initiative = 0f;

    sim.AddEvent(PlayerEvent.Info("! [" + mob.name + "]"));
    Continue();
  }

  public void Continue () {

    if (!PlayerAlive()) {
      // hand player dead situation
    }

    if (!MobAlive()) {
      MobDeath();
      Victory();
    }

    var initiativeProcessor = new InitiativeProcessor(sim.player, currentMob);
    string nextMove = initiativeProcessor.NextMove();

    if (nextMove == InitiativeProcessor.playerIdent) {
      var playerCombatProcessor = new PlayerCombatProcessor(sim);
      playerCombatProcessor.TakeAction();
    } else {
      var mobCombatProcessor = new MobCombatProcessor(sim);
      mobCombatProcessor.TakeAction();
    }

    if (iterationCount >= iterationLimit) {
      sim.AddEvent(PlayerEvent.Info("[DEV] Iteration limit reached! Something went wrong."));
    }

    if (sim.requiresInput) {
      return;
    } else {
      Debug.Log("Iterating battle processor continue");
      ++iterationCount;
      Continue();
    }
  }

  bool PlayerAlive () {
    return sim.player.currentHp > 0f;
  }

  bool MobAlive () {
    return true;
    //Debug.Log("mob's hp is " + hp);
    //return hp > 0f;
  }

  public void MobDeath () {
    sim.AddEvent(PlayerEvent.Info(currentMob.name + " dies!"));
  }

  public void Victory () {

    GainExperience();
    Gold();
    Equipment();
    Consumables();

//    sim.AddEvent(AfterBattleChoices());

    sim.currentMob = null;
  }

  public void Gold () {
    // Gold
    if (tpd.RollPercent(currentMob.goldChance)) {
      var goldGenerator = new GoldGenerator(sim.player, currentMob);
      var amount = goldGenerator.Mob();
      var ev = PlayerEvent.Info(string.Format("{0} gold", amount));
      var trigger = new Trigger(Trigger.Type.PlayerResourceChange);
      trigger.data[Trigger.resourceKey] = Constants.goldAttr;
      trigger.data[Trigger.resourceAmountKey] = amount;

      ev.Triggers.Add(trigger);
      sim.AddEvent(ev);
    }
  }

  public void Equipment () {

    // Chance for loot
    if (tpd.RollPercent(currentMob.lootChance)) {
      var equipmentGenerator = new EquipmentGenerator(sim);
      var eq = equipmentGenerator.Generate();
      sim.AddEvent(PlayerEvent.Loot(eq));
    }

  }

  public void Consumables () {

    if (tpd.RollPercent(currentMob.consumableChance)) {
      //if (true) {
      var env = sim.currentEnvironment;
      var consumableKey = tpd.RollMap(env.consumableChances);
      var consumableGenerator = new ConsumableGenerator(sim);
      var consumable = consumableGenerator.Generate(consumableKey);
      var ev = PlayerEvent.Consumable(consumable);

      sim.AddEvent(ev);
    }
  }

  public void GainExperience () {
    var experienceProcessor = new ExperienceProcessor(sim.player, currentMob);
    var amount = experienceProcessor.ExperienceGain();
    var ev = PlayerEvent.Info(string.Format("+{0} experience", amount));
    var trigger = new Trigger(Trigger.Type.PlayerStatChange);
    //trigger.data[Trigger.statKey] = Stat.xp;
    trigger.data[Trigger.statChangeAmountKey] = amount;
    ev.Triggers.Add(trigger);

    sim.AddEvent(ev);
  }

}
