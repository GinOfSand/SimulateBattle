using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DLL_D20;
using static System.Net.Mime.MediaTypeNames;

namespace SimulateBattle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Assembly asm = Assembly.Load(AssemblyName.GetAssemblyName("DLL_D20.dll"));
            //Module mod = asm.GetModule("DLL_D20.dll");
            //Console.WriteLine("Типы данных:");
            //foreach (Type t in mod.GetTypes())
            //{
            //    Console.WriteLine(t.FullName);
            //}
            //Type characters = mod.GetType("DLL_D20.Characters") as Type;
            //Type D20 = mod.GetType("DLL_D20.D20_Dice") as Type;
            //object dice = Activator.CreateInstance(D20, new object[] { });
            //object cha1 = Activator.CreateInstance(characters, new object[] { "Elf", 45, 45, 14, 2, 2 });
            //object cha2 = Activator.CreateInstance(characters, new object[] { "Human", 55, 55, 12, 4, 1 });
            //Console.WriteLine(characters.GetMethod("ToString").Invoke(cha1, null));
            //Console.WriteLine(characters.GetMethod("ToString").Invoke(cha2, null));
            Characters Elf = new Characters("Elf", 45, 45, 15, 2, 2);
            Characters Human = new Characters("Human", 55, 55, 12, 3, 1);
            Console.WriteLine("БОЙ НАЧИНАЕТСЯ!!!!!");
            Console.Write("Первый Атакует : ");
            D20_Dice dice = new D20_Dice();
            int D20Hum = dice.RollD20();
            int D20Elf = dice.RollD20();
            string ochered = null;
            Characters attaker = null;
            Characters defending = null;
            if(D20Hum >= D20Elf)
            {
               ochered = Human.Name;
                attaker = Human;
                defending = Elf;
                Console.Write(Human.Name+"\n");
            }
            else
            {
                attaker = Elf;
                defending = Human;
                ochered = Elf.Name;
                Console.Write(Elf.Name + "\n");
            }
            int round = 0;
            do {
                round++;
                Console.WriteLine($"Раунд: {round}");
                if(attaker.HitpointCurent <= attaker.HitpointMAx / 100 * 50 && attaker.PotionHealing > 0)
                {
                    Console.WriteLine($"{attaker.Name} использует зелье исцеления");
                    attaker.Healing_chance(dice.RollD20());
                }
                if (defending.HitpointCurent <= defending.HitpointMAx / 100 * 50 && defending.PotionHealing > 0)
                {
                    Console.WriteLine($"{defending.Name} использует зелье исцеления");
                    defending.Healing_chance(dice.RollD20());
                }

                if (attaker.NextBerserkAtak(dice.RollD20()))
                {
                    Console.Write($"{attaker.Name} Яростно атакует ");
                    if(attaker.AttackRating + dice.RollD20() >= defending.ArmorClass)
                    {
                        Console.Write("и попдание!!\n");
                        int damge = dice.RollD12() + dice.RollD12();
                        Console.WriteLine($"Нанося {damge} урона {defending.Name}");
                        defending.HitpointCurent -= damge;
                    }
                    else
                        Console.Write("и промахивается!!\n");
                }
                else
                {
                    Console.Write($"{attaker.Name} Атакует ");
                    if (attaker.AttackRating + dice.RollD20() >= defending.ArmorClass)
                    {
                        Console.Write("и попдание!!\n");
                        int damge = dice.RollD12();
                        Console.WriteLine($"Нанося {damge} урона {defending.Name}");
                        defending.HitpointCurent -= damge;
                    }
                    else
                        Console.Write("и промахивается!!\n");
                }

                if (defending.NextBerserkAtak(dice.RollD20()))
                {
                    Console.Write($"{defending.Name} Яростно атакует ");
                    if (defending.AttackRating + dice.RollD20() >= attaker.ArmorClass)
                    {
                        Console.Write("и попдание!!\n");
                        int damge = dice.RollD10() + dice.RollD10();
                        Console.WriteLine($"Нанося {damge} урона {attaker.Name}");
                        attaker.HitpointCurent -= damge;
                    }
                    else
                        Console.Write("и промахивается!!\n");
                }
                else
                {
                    Console.Write($"{defending.Name} Атакует ");
                    if (defending.AttackRating + dice.RollD20() >= attaker.ArmorClass)
                    {
                        Console.Write("и попдание!!\n");
                        int damge = dice.RollD10();
                        Console.WriteLine($"Нанося {damge} урона {attaker.Name}");
                        attaker.HitpointCurent -= damge;
                    }
                    else
                        Console.Write("и промахивается!!\n");
                }
                Console.WriteLine("Конец Раунда");
                Console.WriteLine($"Текущее состояние {attaker.Name} {attaker.HitpointCurent} из {attaker.HitpointMAx} хитов");
                Console.WriteLine($"Текущее состояние {defending.Name} {defending.HitpointCurent} из {defending.HitpointMAx} хитов\n");
                


            } while(!attaker.Check_the_Death() && !defending.Check_the_Death());
            if (defending.Check_the_Death()){
                Console.WriteLine($"Побеждает: {attaker.Name}");
            }
            else
            {
                Console.WriteLine($"Побеждает: {defending.Name}");
            }
        }
    }
}
