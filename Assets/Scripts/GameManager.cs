using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject NarrationToDeactivate;

    public GameObject BurgerBlockToActivate;
    public GameObject BurgerBlockToDeactivate;
    public GameObject BurgerToDeactivate;

    public GameObject PoutineBlockToActivate;
    public GameObject PoutineBlockToDeactivate;
    public GameObject PoutineToDeactivate;

    public GameObject PizzaBlockToActivate;
    public GameObject PizzaBlockToDeactivate;
    public GameObject PizzaToDeactivate;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneController.previousScene == "Narration")
        {
            NarrationToDeactivate.SetActive(false);

        }
        if (SceneController.previousScene == "Burger")
        {
            BurgerBlockToActivate.SetActive(true);
            BurgerBlockToDeactivate.SetActive(false);
            BurgerToDeactivate.SetActive(false);
        }
        if (SceneController.previousScene == "Poutine")
        {
            PoutineBlockToActivate.SetActive(true);
            PoutineBlockToDeactivate.SetActive(false);
            PoutineToDeactivate.SetActive(false);
        }
        if (SceneController.previousScene == "Pizza")
        {
            PizzaBlockToActivate.SetActive(true);
            PizzaBlockToDeactivate.SetActive(false);
            PizzaToDeactivate.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
