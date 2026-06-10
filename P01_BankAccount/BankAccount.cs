namespace P01_BankAccount;

public class BankAccount
{
    private decimal balance;
    private readonly object balanceLock = new object();

    public BankAccount(decimal initialBalance)
    {
        balance = initialBalance;
    }

    public decimal Balance
    {
        get
        {
            lock (balanceLock)
            {
                return balance;
            }
        }
    }

    public void Deposit(decimal amount)
    {
        lock (balanceLock)
        {
            balance += amount;
        }
    }

    public bool Withdraw(decimal amount)
    {
        lock (balanceLock)
        {
            if (balance < amount)
            {
                return false;
            }
            
            balance -= amount;
            return true;
        }
    }
}