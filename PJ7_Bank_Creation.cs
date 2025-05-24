//º==-V-==º\\
using System.Globalization;

namespace Aprender
{
    public class Program
    {
        public static void Main()
        {
            BankAccountManagement bank = new();
            char[] anim = { '/', '-', '\\', '|' };
            int rotated = 0;

            bank.CreatingAccountVisual();
            Thread.Sleep(5000);
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int i = 0; i < anim.Length; i++)
            {
                Console.Write(anim[i]);
                Thread.Sleep(200);
                Console.Clear();

                if (i == 3)
                {
                    rotated++;
                    i = 0;
                    if (rotated == 4) { break; }
                }
            }

            Console.ResetColor();
            try
            {
                if (bank.AccountState())
                {
                    string decision = String.Empty;
                    do
                    {
                        Console.WriteLine(bank.WelcomeMessage());
                        Console.WriteLine("1.Deposit\n2.Withdraw\n3.See[Money / Info / Transactions]\n4.Send\n");

                        Console.Write("-> ");
                        decision = Console.ReadLine()!.ToLower();
                        Console.Clear();
                        bank.Input(ref decision);
                    }
                    while (decision != "exit" && !String.IsNullOrEmpty(decision));
                }
            }
            catch (NullReferenceException) { throw new NullReferenceException("MAKE SURE TO NOT FAIL WITH SOMETHING OR IT WON'T WORK XDDD"); }
            
            Console.WriteLine("Goodbye!");
        }
    }

    public class User
    {
        public string FullName { get; private set; }
        public string Identification { get; private set; }
        public long PhoneNumber { get; private set; }
        public short PasswordAccount { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public bool StateAccount { get; private set; }
        public Dictionary<string, decimal> Transactions { get; private set; } = new Dictionary<string, decimal>();
        public decimal AccountBalance { get; private set; }

        public User(string nametag, string id, long phoneNumber, short code, DateOnly birth, bool stateAcc)
        {//If an account is created
            this.FullName = nametag;
            this.Identification = id;
            this.PhoneNumber = phoneNumber;
            this.PasswordAccount = code;
            this.BirthDate = birth;
            this.StateAccount = stateAcc;
        }
        public User(bool stateAcc) { this.StateAccount = stateAcc; }
        //If an account is not created

        public decimal AddMoney(decimal? amount = null)
        {
            decimal total = amount ?? 0.00m;
            return AccountBalance += total;
        }

        public decimal WithdrawMoney(decimal? amount = null)
        {
            decimal total = amount ?? 0.00m;
            return AccountBalance -= total;
        }

        public void SendMoney(string name, decimal? amount = null)
        {
            decimal transac = amount ?? 0.00m;
            if (!Transactions.TryAdd(name, transac))
            {
                Transactions[name] += transac;
            }
        }
    }

    public class BankAccountManagement
    {
        private User _data;

        public void CreatingAccountVisual()
        {
            Console.WriteLine("Insert your full name -");
            string fullName = Console.ReadLine()!.ToUpper();
            if (String.IsNullOrEmpty(fullName)) { ErrorMessages("YOU CAN'T LEAVE IT BLANK!"); return; }
            Console.Clear();

            Console.WriteLine("Now your ID -");
            string identification = Console.ReadLine()!.ToUpper();
            if (identification.Length < 4 || identification.Length > 9 || String.IsNullOrEmpty(identification))
            {
                ErrorMessages("\"Error: \\n1.THE MINIMUN IS 5 AND MAXIMUN 9 CHARACTERS!\\n2.YOU CAN'T LEAVE IT BLANK!\"");
                return;
            }
            Console.Clear();

            Console.WriteLine("Then, write your phone number -");
            if (!long.TryParse(Console.ReadLine(), out long phoneNumber)) { ErrorMessages("Error: INCORRECT FORMAT!"); return; }
            Console.Clear();

            //I couldn't handle the exception where you put for example: [ 30th of february == ArgumentOutOfRangeException ]
            Console.WriteLine("Last, insert your birth date (year-month-day) -");
            if (!ushort.TryParse(Console.ReadLine(), out ushort year) || (year > 9999)) { ErrorMessages("Error: INCORRECT FORMAT!"); return; }

            if (!ushort.TryParse(Console.ReadLine(), out ushort month) || month > 12) { ErrorMessages("Error: INCORRECT FORMAT!"); return; }

            if (!ushort.TryParse(Console.ReadLine(), out ushort day) || day > 31) { ErrorMessages("Error: INCORRECT FORMAT!"); return; }
            Console.Clear();

            var date = new DateOnly(year, month, day);

            /* var todayDate = DateOnly.FromDateTime(DateTime.Today);
             * I tried to do that if you're underage you cannot do an account, but
             * I didn't reach out with a solution.
            */
            Console.WriteLine("Do you want to confirm your action? -yes- or -no-");
            CreatingAccount(fullName, identification, phoneNumber, CreatingPasswordAccount(), date);
        }

        public void Input(ref string input)
        {
            string[] words = input.Split(' ', 2);
            string firstWord = string.Empty, secondWord = string.Empty;
            if (words.Length > 1)
            {
                firstWord = words[0].ToLower();
                secondWord = words[1].ToLower();
            }
            else { firstWord = words[0].ToLower(); }

            switch (firstWord)
            {
                case "deposit":
                    Console.Write("How much you want to deposit? (Max. $10.000): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal myNum))
                    {
                        Deposit(myNum);
                    }
                    else { Console.WriteLine(ErrorMessages("INCORRECT FORMAT!")); }
                    break;

                case "withdraw":
                    Console.Write("How much you want to withdraw? (Max. $4.000): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal myNum2) && _data.AccountBalance > 0.00m)
                    {
                        Withdraw(myNum2);
                    }
                    else { Console.WriteLine(ErrorMessages("1.INCORRECT FORMAT!\n2.YOU'RE POOR")); }
                    break;

                case "see":
                    if (secondWord == "money") { Console.WriteLine(GettingMoney() + "\n"); }
                    else if (secondWord == "info") { Console.WriteLine(GettingInfo() + "\n"); }
                    else if (secondWord == "transactions") { ShowingTransactions(); }
                    else { Console.WriteLine("See what?\n"); }

                    break;

                case "send":
                    Console.Write("Enter your password to send => ");
                    short pass = short.Parse(Console.ReadLine());

                    if (CanAccess(pass))
                    {
                        Console.WriteLine("Who you want to send to?");
                        string name = Console.ReadLine()!.ToUpper();

                        Console.Write("How much you want to send? (Max. $5.000): ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal myNum3) && !String.IsNullOrEmpty(name) && _data.AccountBalance > 0.00m)
                        {
                            Send(name, myNum3);
                            Withdraw(myNum3);
                            Console.WriteLine("You succesfully send your money to {0}", name);
                        }
                        else { Console.WriteLine(ErrorMessages("1.INCORRECT FORMAT!\n2.YOU'RE POOR")); }
                    }
                    else { Console.WriteLine(ErrorMessages("ENTER YOUR PASSWORD CORRECTLY!")); }

                    break;
            }
            Console.ResetColor();
            Console.Write("Enter any key to exit ");
            Console.ReadKey();

            Console.Clear();
        }

        public void Deposit(decimal number)
        {
            if ((number > 0.00m) && (number <= 10_000.00m))
            {
                _data.AddMoney(number);
            }
            else { Console.WriteLine(ErrorMessages("INCORRECT AMOUNT!")); }
        }

        public void Withdraw(decimal number)
        {
            if ((number > 0.00m) && (number <= 4_000.00m) && !ReachBelowZero(number))
            {
                _data.WithdrawMoney(number);
            }
            else { Console.WriteLine(ErrorMessages("INCORRECT AMOUNT!")); }
        }

        public void Send(string name, decimal number)
        {
            if ((number > 0.00m) && (number <= 5_000.00m) && !ReachBelowZero(number))
            {
                _data.SendMoney(name, number);
            }
            else { Console.WriteLine(ErrorMessages("INCORRECT AMOUNT!")); }
        }

        public string GettingMoney()
        {
            CultureInfo language = new("en-US");

            Console.ForegroundColor = ConsoleColor.Green;
            string money = String.Format(language, "{0:C2}", _data.AccountBalance);
            return money;
        }

        public string GettingInfo()
        {
            Console.WriteLine("BANK ACCOUNT INFORMATION ---\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            return $"Name: {_data.FullName}\nID: {_data.Identification}\n" +
                $"Birth Date: {_data.BirthDate}\nTel.: {_data.PhoneNumber}";
        }

        public string WelcomeMessage() => $"WELCOME, {_data.FullName}!\n";
        public bool AccountState() => _data.StateAccount;
        public void ShowingTransactions() => GettingTransactions();
        private bool ReachBelowZero(decimal number) => (_data.AccountBalance - number) < 0.00m;
        private bool CanAccess(short pass) => pass.Equals(_data.PasswordAccount);

        private void CreatingAccount(string fullName, string id, long phoneNum, short code, DateOnly birth)
        {
            bool state = false;
            string decision = Console.ReadLine()!.ToLower();
            if (decision.Equals("yes"))
            {
                state = true;
                _data = new(fullName, id, phoneNum, code, birth, state);
            }
            else if (decision.Equals("no"))
            {
                state = false;
                _data = new(stateAcc: state);
                return;
            }
            else { ErrorMessages("INCORRECT FORMAT!"); return; }

            Console.WriteLine("Here is your password: {0}", _data.PasswordAccount);
        }

        private void GettingTransactions()
        {
            CultureInfo lang = new("en-US");
            if (_data.Transactions.Count > 0)
            {
                var myValues = _data.Transactions.Values.ToArray<decimal>();
                decimal totalValues = 0.00m;

                for (int i = 0; i < myValues.Length; i++)
                {
                    totalValues += myValues[i];
                }
                Console.WriteLine("Here are your transactions -");
                decimal myBalanceBefore = _data.AccountBalance + totalValues;

                Console.ForegroundColor = ConsoleColor.Blue;
                foreach (var transaction in _data.Transactions)
                {
                    string money = String.Format(lang, "Amount: {0:C2}", transaction.Value);
                    decimal myBalanceAfter = myBalanceBefore - transaction.Value;

                    Console.WriteLine("Name: {0,5}\n{1,3}\nBalance: {2} | {3}\n------------------------------\n",
                        transaction.Key, money, myBalanceBefore, myBalanceAfter);
                    
                    myBalanceBefore = myBalanceAfter;
                }
            }
            else { Console.WriteLine(ErrorMessages("SEND MONEY!")); }
        }

        private static short CreatingPasswordAccount()
        {//I just wanted something short
            Random rng = new();
            string[] password = new String[4];

            for (int i = 0; i < password.Length; i++)
            {
                int myNum = rng.Next(0, 10);
                password[i] = myNum.ToString();
            }
            string code = String.Concat(password);

            return short.Parse(code);
        }

        private static Exception ErrorMessages(string messg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            return new Exception(messg);
        }
    }
}

