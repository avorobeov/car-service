using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace car_service
{
    class Program
    {
        static void Main(string[] args)
        {
            CarService carService = new CarService(200);

            carService.StartWork();
        }

        class Detail
        {
            public int Price { get; private set; }
            public string Title { get; private set; }

            public Detail(int price,string title)
            {
                Price = price;
                Title = title;
            }
        }

        class Car
        {
            public string Title { get; private set; }
            public bool Engine { get; private set; }
            public bool Injector { get; private set; }
            public bool Battery { get; private set; }
            public bool Wheel { get; private set; }

            public Car(string title, bool engine, bool injector, bool battery, bool wheel)
            {
                Title = title;
                Engine = engine;
                Injector = injector;
                Battery = battery;
                Wheel = wheel;
            }

            public void TryFix(string notServiceability)
            {
                switch (notServiceability)
                {
                    case "Engine":
                        Engine = true;
                        break;

                    case "Injector":
                        Injector = true;
                        break;

                    case "Battery":
                        Battery = true;
                        break;

                    case "Wheel":
                        Wheel = true;
                        break;
                }
            }
        }

        class Customer
        {
            private Car _car;

            public int Money { get; private set; }

            public Customer(Car car, int money)
            {
                _car = car;
                Money = money;
            }

            public Car GetCar()
            {
                return _car;
            }

            public void IncreaseBalance(int money)
            {
                Money += money;
            }

            public void DecreaseBalance(int money)
            {
                Money -= money;
            }
        }

        class CarService
        {
            private List<Detail> _details = new List<Detail>();
            private Queue<Customer> _customers = new Queue<Customer>();

            private int _amountCompensation = 15;
         
            public int Money { get; private set; }

            public CarService(int money)
            {
                Money = money;

                _details.Add(new Detail(100, "Engine"));
                _details.Add(new Detail(100, "Engine"));

                _details.Add(new Detail(30, "Injector"));
                _details.Add(new Detail(30, "Injector"));

                _details.Add(new Detail(50, "Battery"));
                _details.Add(new Detail(50, "Battery"));

                _details.Add(new Detail(10, "Wheel"));
                _details.Add(new Detail(10, "Wheel"));

                _customers.Enqueue(new Customer(new Car("Audi", false, true, true, true), 120));
                _customers.Enqueue(new Customer(new Car("Audi", true, true, false, true), 10));
                _customers.Enqueue(new Customer(new Car("Volkswagen", true, true, false, true), 140));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, false, true), 40));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, true, false), 300));
                _customers.Enqueue(new Customer(new Car("Volkswagen", true, true, true, false), 120));
                _customers.Enqueue(new Customer(new Car("Audi", true, false, true, true), 120));
                _customers.Enqueue(new Customer(new Car("Audi", true, false, true, true), 10));
                _customers.Enqueue(new Customer(new Car("Volkswagen", true, true, false, true), 140));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, false, true), 40));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, false, true), 300));
                _customers.Enqueue(new Customer(new Car("Volkswagen", false, false, true, true), 120));
                _customers.Enqueue(new Customer(new Car("Audi", true, false, true, true), 120));
                _customers.Enqueue(new Customer(new Car("Audi", true, false, true, true), 10));
                _customers.Enqueue(new Customer(new Car("Volkswagen", true, true, false, true), 140));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, false, true), 40));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, false, true), 300));
                _customers.Enqueue(new Customer(new Car("Volkswagen", false, false, true, true), 120));
                _customers.Enqueue(new Customer(new Car("Audi", true, false, true, true), 120));
                _customers.Enqueue(new Customer(new Car("Audi", true, false, true, true), 10));
                _customers.Enqueue(new Customer(new Car("Volkswagen", true, true, false, true), 140));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, false, true), 40));
                _customers.Enqueue(new Customer(new Car("BMW", true, true, false, true), 300));
                _customers.Enqueue(new Customer(new Car("Volkswagen", false, false, true, true), 120));
            }

            public void StartWork()
            {
                while (_details.Count != 0 && _customers.Count != 0 && Money > 0)
                {
                    Customer customer = _customers.Dequeue();
                    Car car = customer.GetCar();

                    int repairPrice;

                    string notServiceability = GetNotServiceability(car);

                    ShowNotServiceability(notServiceability, out repairPrice);

                    if (GetDecisions())
                    {
                        if (GetAvailabilityDetail(notServiceability))
                        {
                            if (customer.Money >= repairPrice)
                            {
                                Detail detail1 = _details.Find(detail => detail.Title.Contains(notServiceability));

                                customer.DecreaseBalance(repairPrice);

                                Money += repairPrice;

                                car.TryFix(notServiceability);

                                _details.Remove(detail1);

                                ShowMessage("Ваша машина успешно отремонтирована", ConsoleColor.Cyan);
                            }
                            else
                            {
                                ShowMessage("У вас недостаточно денег!", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            Money -= _amountCompensation;

                            customer.IncreaseBalance(_amountCompensation);

                            ShowMessage("Извините даной детали нет на складе мы готовы выплатить вам компенсацию\n", ConsoleColor.Cyan);
                        }
                    }
                    else
                    {
                        ShowMessage("Всего хорошего были рады вас видеть \n", ConsoleColor.Gray);
                    }
                  
                    ShowInfo();

                    Console.ReadKey();
                    Console.Clear();
                }
                ShowMessage("Извините сервис временно закрыт ", ConsoleColor.Red);
            }

            private bool GetDecisions()
            {
                ShowMessage("\n\nЕли вы согласны на ремонт нажмите 1 \nЕсли нет нажмите 2", ConsoleColor.Green);

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        return true;

                    case "2":
                        return false;
                }

                return false;
            }

            private bool GetAvailabilityDetail(string notServiceability)
            {
                return _details.Find(title => title.Title.Contains(notServiceability)) != null;
            }

            private void ShowNotServiceability(string notServiceability, out int repairPrice)
            {

                repairPrice = GetPriceDetail(notServiceability) + GetPriceWork(notServiceability);

                ShowMessage($"Добрый день! \nУ вас сломан: {notServiceability} \nСтоимость его замены будет: {repairPrice}", ConsoleColor.Red);

            }

            private string GetNotServiceability(Car car)
            {
                if (car.Engine != true)
                {
                    return "Engine";
                }
                else if (car.Injector != true)
                {
                    return "Injector";
                }
                else if (car.Battery != true)
                {
                    return "Battery";
                }
                else if (car.Wheel != true)
                {
                    return "Wheel";
                }

                return "У вас всё хорошо с машиной!";
            }
      
            private int GetPriceDetail(string failure)
            {
                switch (failure)
                {
                    case "Engine":
                        return 70;

                    case "Injector":
                        return 20;

                    case "Battery":
                        return 10;

                    case "Wheel":
                        return 5;
                }

                return 0;
            }
      
            private int GetPriceWork(string failure)
            {
                switch (failure)
                {
                    case "Engine":
                        return 70;

                    case "Injector":
                        return 20;

                    case "Battery":
                        return 10;

                    case "Wheel":
                        return 5;
                }

                return 0;
            }

            private void ShowInfo()
            {
                ShowMessage($"На вашем балансе {Money}\nОсталось Клиентов {_customers.Count}\nОсталось Деталей {_details.Count}", ConsoleColor.Magenta);
            }

            private void ShowMessage(string message, ConsoleColor color)
            {
                ConsoleColor preliminaryColor = Console.ForegroundColor;

                Console.ForegroundColor = color;
                Console.WriteLine(message);

                Console.ForegroundColor = preliminaryColor;
            }
        }
    }
}
