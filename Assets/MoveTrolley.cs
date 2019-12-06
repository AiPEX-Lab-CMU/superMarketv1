using System.Collections;
using System.Collections.Generic;
using GameCreator.Core;
using GameCreator.Playables;
using GameCreator.Characters;
using UnityEngine;

public class MoveTrolley : MonoBehaviour
{
    bool cartMoving;
    bool shoppingComplete;
    ShoppingList sl;
    MarketItems mi;
    string nextItem = "";
    GameObject targetObj;
    SendMessage messageSender;
    // Start is called before the first frame update
    void Start()
    {
        cartMoving = false;
        shoppingComplete = false;
        sl = transform.GetComponent<ShoppingList>();
        mi = transform.GetComponentInParent<MarketItems>();
        messageSender = (SendMessage)GameObject.Find("messageSender").GetComponent(typeof(SendMessage));
        //GameObject.Find("Product_potatoes").GetComponentInChildren<OBJExporter>().Export("temp.obj");
        //GameObject potato = GameObject.Find("Product_potatoes (1)");
        //potato.AddComponent<Rigidbody>();
        //potato.GetComponent<Rigidbody>().useGravity = false;
        //potato.AddComponent<BoxCollider>();
        //Mesh mesh = potato.GetComponentInChildren<MeshFilter>().sharedMesh;
        //Mesh mesh1 = Instantiate(mesh);
        ////Mesh mesh = potato.GetComponentInChildren<Mesh>();
        //MeshCollider meshcld = potato.AddComponent<MeshCollider>();
        //meshcld.sharedMesh = mesh1;
        //meshcld.convex = true;
        //meshcld.isTrigger = true;
        //Vector3 newSize = new Vector3(mesh.bounds.size.x * transform.localScale.x * 0.1f, mesh.bounds.size.y * transform.localScale.y * 0.1f, mesh.bounds.size.z * transform.localScale.z * 0.1f);
        //potato.GetComponent<BoxCollider>().size = new Vector3(0.1f,0.1f,0.1f);
        //transform.gameObject.AddComponent<Trigger>();
        //Trigger trigger = gameObject.GetComponent<Trigger>();
        //gameObject.AddComponent<Actions>();
        //Actions actions = gameObject.GetComponent<Actions>();
        //ActionTransformMove moveAction = new ActionTransformMove();
        //moveAction.target = new TargetGameObject(TargetGameObject.Target.GameObject);
        //IActionsList actionsList = new IActionsList();
        //actionsList.actions = new IAction[1];
        //actionsList.actions[0] = moveAction;
        //actions.actionsList = actionsList;
        //gameObject.AddComponent<Actions>(actions);
    }

    // Update is called once per frame
    void Update()
    {
        if (!shoppingComplete)
        {
            if (!cartMoving)
            {
                cartMoving = true;
                if (sl.items.Count == 0)
                {

                    shoppingComplete = true;
                    Debug.Log("Shopping Complete");
                }
                else
                {
                    nextItem = sl.items.Dequeue();
                    moveCart();
                }
            }
        }
    }

    void moveCart()
    {
        targetObj = GameObject.Find(nextItem);
        Actions actions = GameObject.Find("MoveToObject").GetComponent<Actions>();
        ActionCharacterMoveTo charMoveAction = actions.actionsList.actions[0].GetComponent<ActionCharacterMoveTo>();
        charMoveAction.moveTo = ActionCharacterMoveTo.MOVE_TO.Transform;
        charMoveAction.transform = targetObj.transform;
        ActionTransformMove objMove = actions.actionsList.actions[1].GetComponent<ActionTransformMove>();
        objMove.target.target = TargetGameObject.Target.GameObject;
        objMove.target.gameObject = targetObj.gameObject;
        ActionRigidbody actionRigidbody = actions.actionsList.actions[2].GetComponent<ActionRigidbody>();
        actionRigidbody.target.target = TargetGameObject.Target.GameObject;
        actionRigidbody.target.gameObject = targetObj.gameObject;
        actions.Execute();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.parent = null;
        collision.transform.SetParent(transform);
        //GameObject.Find("screenshotCam").GetComponent<screenshotScript>().takeScreenShot();
        MeshFilter mf;
        if ((mf = targetObj.GetComponent<MeshFilter>()) != null)
        {

            ExportObj eo = targetObj.AddComponent<ExportObj>();
            eo.CreateFile(targetObj);
        }
        else
        {

            mf = targetObj.GetComponentInChildren<MeshFilter>();
            Debug.Log(mf.gameObject.name);
            //GameObject child = targetObj.GetComponentInChildren<GameObject>();
            ExportObj eo = mf.gameObject.AddComponent<ExportObj>();
            eo.CreateFile(mf.gameObject);
        }
        cartMoving = false;
        //call sendToPython function
        messageSender.sendBytes("001", 0, 0, "000");
        Debug.Log("Picked up " + mi.searchProduct(collision.gameObject));
    }

    public void removeTrigger()
    {
        //Debug.Log("entered");
        targetObj.GetComponent<BoxCollider>().isTrigger = false;
    }
}