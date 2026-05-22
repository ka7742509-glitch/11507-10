using System;

namespace ForgeOfHeroes
{ 
    public abstract class Hero
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
    }

    public class Warrior : Hero { public string WeaponName { get; set; } }
    public class Mage : Hero { public int Mana { get; set; } }
    public class Archer : Hero { public double ShotDistance { get; set; } }
    public interface IStepName<T> where T : Hero { IStepHealth<T> SetName(string name); }
    public interface IStepHealth<T> where T : Hero { IStepAttack<T> SetHealth(int health); }
    public interface IStepAttack<T> where T : Hero { IStepUnique<T> SetAttack(int attack); }
    
    public interface IStepUnique<T> where T : Hero 
    { 
        IStepBuild<T> SetWeaponName(string weapon); 
        IStepBuild<T> SetMana(int mana); 
        IStepBuild<T> SetShotDistance(double distance); 
    }
    
    public interface IStepBuild<T> where T : Hero { T Build(); }

    public class HeroBuilder<T> : IStepName<T>, IStepHealth<T>, IStepAttack<T>, IStepUnique<T>, IStepBuild<T> 
        where T : Hero, new()
    {
        private string _name;
        private int _health;
        private int _attack;
        private string _weaponName;
        private int _mana;
        private double _shotDistance;

        public IStepHealth<T> SetName(string name) { _name = name; return this; }
        
        public IStepAttack<T> SetHealth(int health) { _health = health; return this; }
        
        public IStepUnique<T> SetAttack(int attack) { _attack = attack; return this; }

        public IStepBuild<T> SetWeaponName(string weapon) 
        { 
            if (typeof(T) != typeof(Warrior)) throw new InvalidOperationException("SetWeaponName можно вызывать только для Воина.");
            _weaponName = weapon; return this; 
        }

        public IStepBuild<T> SetMana(int mana) 
        { 
            if (typeof(T) != typeof(Mage)) throw new InvalidOperationException("SetMana можно вызывать только для Мага.");
            _mana = mana; return this; 
        }

        public IStepBuild<T> SetShotDistance(double distance) 
        { 
            if (typeof(T) != typeof(Archer)) throw new InvalidOperationException("SetShotDistance можно вызывать только для Лучника.");
            _shotDistance = distance; return this; 
        }

        public T Build()
        {
            var hero = new T 
            { 
                Name = _name, 
                Health = _health, 
                Attack = _attack 
            };

            if (hero is Warrior w) w.WeaponName = _weaponName;
            if (hero is Mage m) m.Mana = _mana;
            if (hero is Archer a) a.ShotDistance = _shotDistance;

            return hero;
        }
    }

    public static class Forge
    {
        public static IStepName<T> CreateHero<T>() where T : Hero, new() => new HeroBuilder<T>();
    }

    class Program
    {
        static void Main()
        {
            var warrior = Forge.CreateHero<Warrior>()
                .SetName("Арагорн")
                .SetHealth(100)
                .SetAttack(25)
                .SetWeaponName("Андурил")
                .Build();

            var mage = Forge.CreateHero<Mage>()
                .SetName("Гэндальф")
                .SetHealth(80)
                .SetAttack(30)
                .SetMana(150)
                .Build();

            var archer = Forge.CreateHero<Archer>()
                .SetName("Леголас")
                .SetHealth(90)
                .SetAttack(35)
                .SetShotDistance(500.5)
                .Build();

            Console.WriteLine($"{warrior.Name} | HP: {warrior.Health} | ATK: {warrior.Attack} | Weapon: {warrior.WeaponName}");
            Console.WriteLine($"{mage.Name} | HP: {mage.Health} | ATK: {mage.Attack} | Mana: {mage.Mana}");
            Console.WriteLine($"{archer.Name} | HP: {archer.Health} | ATK: {archer.Attack} | Distance: {archer.ShotDistance}");
        }
    }
}