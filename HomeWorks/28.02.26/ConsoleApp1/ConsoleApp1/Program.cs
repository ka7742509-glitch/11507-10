using System.Numerics;

namespace ConsoleApp1
{
    public class Order<T> where T : INumber<T>
    {
        public int Id { get; internal set; }
        public T BasePrice { get; internal set; }
        public T FinalPrice { get; internal set; }
    }

    public interface IIdStep<T> where T : INumber<T> 
    { 
        IPriceStep<T> SetId(int id); 
    }

    public interface IPriceStep<T> where T : INumber<T> 
    { 
        IFinalStep<T> SetBasePrice(T price); 
    }

    public interface IFinalStep<T> where T : INumber<T> 
    { 
        Order<T> Build(); 
    }

    public class OrderBuilder<T> : IIdStep<T>, IPriceStep<T>, IFinalStep<T> where T : INumber<T>
    {
        private readonly Order<T> _order = new();

        public IPriceStep<T> SetId(int id) 
        { 
            _order.Id = id; 
            return this;
        }

        public IFinalStep<T> SetBasePrice(T price) 
        { 
            _order.BasePrice = price; 
            return this;
        }

        public Order<T> Build() 
        { 
            _order.FinalPrice = _order.BasePrice; 
            return _order; 
        }
    }

    public abstract class OrderHandler<T> where T : INumber<T>
    {
        protected OrderHandler<T>? _next;

        public OrderHandler<T> SetNext(OrderHandler<T> handler)
        {
            _next = handler;
            return handler;
        }

        public abstract void Handle(Order<T> order);

        protected void Next(Order<T> order) => _next?.Handle(order);
    }

    public class DiscountHandler<T> : OrderHandler<T> where T : INumber<T>
    {
        private readonly T _discountAmount;
        public DiscountHandler(T discountAmount) => _discountAmount = discountAmount;

        public override void Handle(Order<T> order)
        {
            order.FinalPrice -= _discountAmount;
            Console.WriteLine($"Вычтено {_discountAmount}. Текущая цена: {order.FinalPrice}");
            Next(order);
        }
    }

    public class TaxHandler<T> : OrderHandler<T> where T : INumber<T>
    {
        public override void Handle(Order<T> order)
        {
            T taxMultiplier = T.CreateChecked(1.2); 
            order.FinalPrice *= taxMultiplier;
            Console.WriteLine($"Текущая цена: {order.FinalPrice}");
            Next(order);
        }
    }

    public class ValidationHandler<T> : OrderHandler<T> where T : INumber<T>
    {
        public override void Handle(Order<T> order)
        {
            if (order.FinalPrice < T.Zero)
                throw new InvalidOperationException($"Валидация не пройдена: итоговая цена {order.FinalPrice} отрицательная!");
            
            Console.WriteLine($"Цена корректна. Итог: {order.FinalPrice}");
        }
    }

    public class OrderProcessor
    {
        public static void Process<T>(Order<T> order) where T : INumber<T>
        {
            var chain = new DiscountHandler<T>(T.CreateChecked(100))
                .SetNext(new TaxHandler<T>())
                .SetNext(new ValidationHandler<T>());

            chain.Handle(order);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Тест Order<decimal>");
            var orderDecimal = new OrderBuilder<decimal>()
                .SetId(101)
                .SetBasePrice(1000m)
                .Build();
            OrderProcessor.Process(orderDecimal);

            Console.WriteLine("Тест Order<double>");
            var orderDouble = new OrderBuilder<double>()
                .SetId(202)
                .SetBasePrice(5000.0)
                .Build();
            OrderProcessor.Process(orderDouble);

            Console.WriteLine("Тест валидации");
            try
            {
                var orderFail = new OrderBuilder<decimal>()
                    .SetId(303)
                    .SetBasePrice(50m) 
                    .Build();
                OrderProcessor.Process(orderFail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Поймано исключение: {ex.Message}");
            }
        }
    }
}