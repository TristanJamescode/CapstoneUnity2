using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Reference https://www.youtube.com/watch?v=-VkezxxjsSE and my dad
/// </summary>
public class StateMachine : MonoBehaviour
{
    public BaseState currentState = null;
    public void setInitState(BaseState initState)
    {
        currentState = initState;
    }
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
            //Debug.Log("currentstate is "+currentState.name);
            currentState.Transactions();
        }
    }
    public void ChangeState(BaseState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
        //Debug.Log("Changed to " + currentState.name);
    }
}
public class BaseState
{
    public string name;
    protected StateMachine stateMachine;
    private List<Transaction> transactions= new List<Transaction>();
    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }
    public virtual void OnEnter() { }
    public virtual void Update() { }
    public virtual void OnExit() { }
    public virtual void Transactions()
    {
        if (transactions.Count == 0) return;
        foreach(Transaction transaction in transactions)
        {
            if (transaction.CheckTransaction())
            {
                stateMachine.ChangeState(transaction.TransactionSuccess());
                return;
            }
        }
        return;
    }
    public void addTransaction(Transaction transaction)
    {
        this.transactions.Add(transaction);
    }
}

public class Transaction
{
    private List<TransactionCondition> triggers = new List<TransactionCondition>();
    private BaseState nextState;
    public Transaction(BaseState nextState)
    {
        //this.trigger = trigger;
        this.nextState = nextState;
    }
    public virtual bool CheckTransaction()
    {
        bool returnbool = true;
        foreach(TransactionCondition trigger in triggers) //all trigger must be complished
        {
            if (trigger.needTrue != trigger.TriggerCheck()) returnbool = false;
        }
        return returnbool;
    }
    public virtual BaseState TransactionSuccess() {
        return nextState;
    }
    public void addCondition(TransactionCondition condition,bool needTrue)
    {
        condition.needTrue = needTrue;
        this.triggers.Add(condition);
    }
}
abstract public class TransactionCondition
{
    public bool needTrue;
    abstract public bool TriggerCheck();
}