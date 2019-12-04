using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketItems : MonoBehaviour
{

    public List<string> itemList = new List<string>();
    public Dictionary<string, List<GameObject>> map = new Dictionary<string, List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject child = GameObject.Find("shelves");
        foreach(Transform tf in child.GetComponentsInChildren<Transform>())
        {
            string thisItem = tf.gameObject.name;
            if(thisItem.ToLower().Contains("product") && !thisItem.Contains("LOD"))
            {
                //if (thisItem.ToLower().Contains("mayo"))
                //{
                    Rigidbody rb = tf.gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    BoxCollider bc = tf.gameObject.AddComponent<BoxCollider>();
                    bc.size = new Vector3(0.1f, 0.1f, 0.1f);
                    bc.isTrigger = true;
                //}
                string tmp = thisItem.Substring(thisItem.IndexOf('_')+1);
                int index = tmp.IndexOfAny(new char[]{'0','1','2','3','4','5','6','7','8','9',' '});
                string product;
                if (index != -1)
                    product = tmp.Substring(0, index);
                else
                    product = tmp;
                //if (!itemList.Contains(product))
                //{
                //    itemList.Add(product);
                //}
                if(map.ContainsKey(product))
                {
                    map[product].Add(tf.gameObject);
                }
                else
                {
                    List<GameObject> temp = new List<GameObject>();
                    temp.Add(tf.gameObject);
                    map.Add(product, temp);
                }
            }
        }
        foreach (KeyValuePair<string, List<GameObject>> pair in map)
        {
            Debug.Log(pair.Key);
        }
        Debug.Log(map.Count);
    }

    public string searchProduct(GameObject obj)
    {
        foreach(KeyValuePair<string, List<GameObject>> pair in map)
        {
            if (pair.Value.Contains(obj))
                return pair.Key;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
