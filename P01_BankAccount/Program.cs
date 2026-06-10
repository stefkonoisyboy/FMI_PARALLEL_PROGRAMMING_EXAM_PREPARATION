namespace P01_BankAccount;

class Program
{
    static BankAccount account = new BankAccount(1000);
    
    static void Main(string[] args)
    {
        Thread t1 = new Thread(DepositWorker);
        Thread t2 = new Thread(WithdrawWorker);
        Thread t3 = new Thread(WithdrawWorker);
        t1.Start(); t2.Start(); t3.Start();
        t1.Join(); t2.Join(); t3.Join();
        Console.WriteLine($"Final balance: {account.Balance}");
    }

    static void DepositWorker()
    {
        for (int i = 0; i < 100; i++)
        {
            account.Deposit(10);
        }
    }

    static void WithdrawWorker()
    {
        for (int i = 0; i < 100; i++)
        {
            if (account.Withdraw(15) is false)
            {
                Console.WriteLine("Insufficient funds.");
            }
        }
    }
}