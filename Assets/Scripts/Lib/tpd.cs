using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Tavernpunk Unity3d Helpers
 * Brit Gardner
 * http://britg.com
 * @britg
 *
 * A set of helper classes that can be dropped into a
 * Unity3D assets folder.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

 */
public class tpd {


  /*
   * Strings
   */

  public static bool BeginsWith (string s, string match) {
    if (s.Length < match.Length) {
      return false;
    }

    return (s.Substring(0, match.Length) == match);
  }

  public static string AddOrdinal (int num) {
    if (num <= 0) return num.ToString();

    switch (num % 100) {
      case 11:
      case 12:
      case 13:
        return num + "th";
    }

    switch (num % 10) {
      case 1:
        return num + "st";
      case 2:
        return num + "nd";
      case 3:
        return num + "rd";
      default:
        return num + "th";
    }

  }

  public static string AddSign (int n) {
    if (n > 0) return "+" + n;
    return "-" + Mathf.Abs(n);
  }

  /*
   * Randomization / Roll Percent
   */

  public static string RollMap (Hashtable hash) {
    float sum = 0f;
    foreach (DictionaryEntry pair in hash) {
      sum += (float)pair.Value;
    }

    float rand = Random.Range(0f, sum);

    float running = 0;
    string chosen = null;
    foreach (DictionaryEntry pair in hash) {
      running += (float)pair.Value;
      if (rand <= running) {
        chosen = (string)pair.Key;
        break;
      }
    }

    return chosen;
  }

  public static string RollMap (Dictionary<string, float> dict) {
    float sum = 0f;
    foreach (KeyValuePair<string, float> pair in dict) {
      sum += (float)pair.Value;
    }

    float rand = Random.Range(0f, sum);

    float running = 0;
    string chosen = null;
    foreach (KeyValuePair<string, float> pair in dict) {
      running += (float)pair.Value;
      if (rand <= running) {
        chosen = (string)pair.Key;
        break;
      }
    }

    return chosen;
  }

  public static bool RollPercent (float chance) {
    float rand = Random.Range(0f, 100f);
    return rand < chance;
  }

  public static float RollRange (RangeAttribute range) {
    return Random.Range(range.min, range.max);
  }

  /*
   * Colors
   */

  public static string ColorToHex (Color32 color) {
    string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
    return hex;
  }

  public static Color HexToColor (string hex) {
    byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
    byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
    byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
    return new Color32(r, g, b, 255);
  }

}

class Notification {

  public string name;
  public Hashtable data;

  public Notification (string aName) {
    name = aName;
  }

  public Notification (string aName, Hashtable aData) {
    name = aName;
    data = aData;
  }

  public object Data (string key) {
    return data[key];
  }
}

public class NotificationCenter : MonoBehaviour {

  private static NotificationCenter instance;


  public Hashtable notifications = new Hashtable();

  public static NotificationCenter Instance {
    get {
      if (instance == null) {
        instance = new GameObject("NotificationCenter").AddComponent<NotificationCenter>();
      }

      return instance;
    }
  }

  public void OnApplicationQuit () {
    instance = null;
  }


  public static void AddObserver (Component observer, string name) {
    if (name == null || name == "") {
      Debug.Log("Null name specificed for notification in AddObserver.");
      return;
    }
    if (Instance.notifications.Contains(name) == false) {
      Instance.notifications[name] = new ArrayList();
    }

    ArrayList notifyList = (ArrayList)Instance.notifications[name];

    if (!notifyList.Contains(observer.gameObject)) {
      notifyList.Add(observer.gameObject);
    }

  }


  public static void RemoveObserver (Component observer, string name) {
    ArrayList notifyList = (ArrayList)Instance.notifications[name];

    if (notifyList != null) {
      if (notifyList.Contains(observer.gameObject)) {
        notifyList.Remove(observer.gameObject);
      }
      if (notifyList.Count == 0) {
        Instance.notifications.Remove(name);
      }
    }
  }

  public static void PostNotification (string aName) {
    PostNotification(aName, null);
  }

  public static void PostNotification (string aName, Hashtable aData) {
    PostNotification(new Notification(aName, aData));
  }

  private static void PostNotification (Notification aNotification) {
    if (aNotification.name == null || aNotification.name == "") {
      Debug.Log("Null name sent to PostNotification.");
      return;
    }

    ArrayList notifyList = (ArrayList)Instance.notifications[aNotification.name];

    if (notifyList == null) {
      Debug.Log("Notify list not found in PostNotification for " + aNotification.name);
      return;
    }

    ArrayList observersToRemove = new ArrayList();

    foreach (GameObject observerGameObject in notifyList) {
      if (!observerGameObject) {
        observersToRemove.Add(observerGameObject);
      } else {
        observerGameObject.SendMessage(aNotification.name, aNotification, SendMessageOptions.DontRequireReceiver);
      }
    }

    foreach (object observerGameObject in observersToRemove) {
      notifyList.Remove(observerGameObject);
    }
  }
}
