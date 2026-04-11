namespace PrinterEventArgs
{
    public class PrinterEventArgs : EventArgs
    {
        public int PagesRequested { get; set; }
        public int PagesAvailable { get; set; }
    
        public PrinterEventArgs(int requested, int available)
        {
            PagesRequested = requested;
            PagesAvailable = available;
        }
    }

    public class Printer
    {
        private int _paperAmount = 10;
    
        public event EventHandler<PrinterEventArgs> PaperOut;
    
        public void Print(int pages)
        {
            if (pages > _paperAmount)
            {
                PaperOut?.Invoke(this, new PrinterEventArgs(pages, _paperAmount));
                return;
            }
            
            _paperAmount -= pages;
            Console.WriteLine($"Успешно напечатано страниц: {pages}. Осталось бумаги: {_paperAmount}");
        }
    
        public void Refill(int amount)
        {
            if (amount <= 0) return;
        
            _paperAmount += amount;
            Console.WriteLine($"Принтер заправлен на {amount} листов. Всего бумаги: {_paperAmount}");
        }
    
        public int GetPaperAmount() => _paperAmount;
    }
}
