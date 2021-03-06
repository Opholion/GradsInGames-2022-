using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _abilityDatabase;
    [SerializeField] private RectTransform _gameObjectParent;
    [SerializeField] private GameObject _baseGameObjectClass;
    [SerializeField] string StartingAbilityName = "Explosion";
    private List<GameObject> AbilityClasses;


    GameObject _currentObjectReference;
    AbilityData _abilityDataRef;
    Component _currentComponent;    // Start is called before the first frame update
    Unit _playerRef;

    void Start()
    {
        AbilityClasses = new List<GameObject>();

        //To allow for testing, the alpha must be visible. The given scene, however, assumes all UI is invisible until set otherwise. This will allow for both criteria to be met.
        GetComponent<CanvasGroup>().alpha = 0;


    }


    


    private void FixedUpdate()
    {
        if (_playerRef == null)
        {
            _playerRef = worldManager.instance.GetPlayer();
            if (!_playerRef) return;
#if UNITY_EDITOR

            //Allow ability tests in debugging
            if(false)
            if (_abilityDatabase.Length < 1)
            {
                BaseAbility temp;
                for (int i = 0; i < 25; ++i)
                {
                    temp = new PunchAbility();
                    AddItem(temp);
                }
            }
            else
            {
                int databaseInt = 0;
                for (int i = 0; i < 25; ++i)
                {
                    AddItem(_abilityDatabase[databaseInt]);
                    ++databaseInt;
                    if (databaseInt >= _abilityDatabase.Length) databaseInt = 0;
                }
            }
#endif


            //Currently the game is quite basic so only a single ability is needed. This could quite easily be added to a Unit which, on death, gives the player a bound ability.
            foreach (GameObject ability in _abilityDatabase)
            {
                if (ability.name.Contains(StartingAbilityName))
                {
                    AddItem(ability);
                    break;
                }
            }
        }

    }
    //Overloaded function to allow the user to either specify a generic class or a specific prefab.
    public void AddItem(BaseAbility ability)
    {
        //Ability is only for holding the exact inherited class type. Figure out what it is and add a component based on it. The prefab should hold a script with any needed unique data.
        _currentObjectReference = Instantiate(_baseGameObjectClass, _gameObjectParent.transform);

        _abilityDataRef = _currentObjectReference.GetComponent<AbilityData>();
        _currentComponent = _currentObjectReference.AddComponent(ability.GetType());
        (_currentComponent as BaseAbility).Setup(_abilityDataRef, _playerRef);
        AbilityClasses.Add(_currentObjectReference);
    }

    public void AddItem(GameObject prefab)
    { 
       

        _currentObjectReference = Instantiate(prefab, _gameObjectParent.transform);
        _abilityDataRef = _currentObjectReference.GetComponent<AbilityData>();

        _currentComponent = _currentObjectReference.GetComponent<BaseAbility>();
        if (_currentComponent == null)
            _currentComponent = _currentObjectReference.AddComponent<BaseAbility>();


        (_currentComponent as BaseAbility).Setup(_abilityDataRef, _playerRef,3,2);
        AbilityClasses.Add(_currentObjectReference);
    }

    public void RemoveObject(int ID)
    {
        if (ID > 0 && ID < AbilityClasses.Count)
            AbilityClasses.RemoveAt(ID);
    }
}
