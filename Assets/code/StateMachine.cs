using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Reference https://www.youtube.com/watch?v=-VkezxxjsSE and my dad
/// </summary>
public class StateMachine : MonoBehaviour
{
    BaseState currentState = null;
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
            currentState.Transactions();
        }
    }
    public void ChangeState(BaseState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
public class BaseState
{
    public string name;
    protected StateMachine stateMachine;
    List<Transaction> transactions= new List<Transaction>();
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
    private BaseState nextState;
    public Transaction(BaseState nextState)
    {
        this.nextState = nextState;
    }
    public virtual bool CheckTransaction()
    {
        return false;
    }
    public virtual BaseState TransactionSuccess() {
        return nextState;
    }
}
public class Trigger
{
}