using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{ 
    public Queue<string> items;

    // Start is called before the first frame update
    void Start()
    {
        items = new Queue<string>();
        items.Enqueue("Product_onion (1)");
        items.Enqueue("Product_frozen_chips01 (9)");
        items.Enqueue("Product_Ketchup");
        items.Enqueue("Product_yogurt01 (1)");
        items.Enqueue("Product_potatoes");
        items.Enqueue("Product_mayo01 (1)");
    }

    void generateShoppingList()
    {
        System.Random rand = new System.Random();
        int index = rand.Next(4);
        ProfileController pc = gameObject.GetComponentInParent<ProfileController>();
        KeyValuePair<string, List<double>> prob = pc.getProfile(index);
        double final_prob = rand.NextDouble();
        for( int i=0; i<prob.Value.Count;i++)
        {
            int rand_var = rand.Next(1);
            if (prob.Value[i] >= (rand_var * final_prob))
                items.Enqueue(pc.items[i]);
        }
        //make sure the list in the order of the shelves
        //get a random number from 0-4 to determine the profile
        //generate a random number of the probability.
        //for each item in the list check if random(0,1) * probability of that item > the random number generated above, the add to the shopping list
    }

    // Update is called once per frame
    void Update()
    {
    }
}
