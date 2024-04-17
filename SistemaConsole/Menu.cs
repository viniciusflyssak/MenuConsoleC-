using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaConsole
{
    internal class Hint
    {
        public String Texto {  get; set; }
        public int Col {  get; set; }
        public int Lin { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public Hint(string texto, int col, int lin) { 
            this.Texto = texto;
            this.Col = col;
            this.Lin = lin;
            this.BackgroundColor = ConsoleColor.Black;
            this.ForegroundColor = ConsoleColor.Blue;
        }
    }

    internal class MenuItem
    {   
        public string Rotulo { get; set; }
        public int Col { get; set; }
        public int Lin { get; set; }

        public bool SubItemAberto { get; set; }
        public ConsoleColor ItemForeground { get; set; }
        public ConsoleColor ItemBackground { get; set; }
        public ConsoleColor SelectorForeground { get; set; }
        public ConsoleColor SelectorBackground { get; set; }

        public Hint Hint { get; set; }

        public List<MenuItem> SubItens { get; set; }

        public event EventHandler<string> EventoClick;
        public MenuItem()
        {
            SubItemAberto = false;
            SubItens = new List<MenuItem>();
            ItemForeground = ConsoleColor.White;
            ItemBackground = ConsoleColor.Black;
            SelectorBackground = ConsoleColor.Blue;
            SelectorForeground = ConsoleColor.Yellow;
        }

        public void Show()
        {
            Console.BackgroundColor = ItemBackground;
            Console.ForegroundColor = ItemForeground;
            Console.SetCursorPosition(Col, Lin);
            Console.Write(Rotulo);
            Console.SetCursorPosition(Col, Lin);
        }

        public void ShowSelector()
        {
            Console.BackgroundColor = SelectorBackground;
            Console.ForegroundColor = SelectorForeground;
            Console.SetCursorPosition(Col, Lin);
            Console.Write(Rotulo);
            if (Hint != null)
            {
                Console.BackgroundColor = Hint.BackgroundColor;
                Console.ForegroundColor = Hint.ForegroundColor;
                Console.SetCursorPosition(0, Hint.Lin);
                Console.Write(new string(' ', Console.BufferWidth));
                Console.SetCursorPosition(Hint.Col, Hint.Lin);
                Console.Write(Hint.Texto);
            }
            Console.SetCursorPosition(Col, Lin);
        }

        public void ExecutarEventoClick()
        {
            EventoClick?.Invoke(this, Rotulo);
        }

    }
    internal class Menu
    {
        public List<MenuItem> Items { get; set; }
        public int PosAtual { get; set; }
        public int PosSubItem { get; set; }
        public string Titulo { get; set; }
        public ConsoleColor TitleForeground { get; set; }
        public ConsoleColor TitleBackground { get; set; }
        public event EventHandler<string> EventoOpcaoTrocada;

        public Menu(string titulo)
        {
            Items = new List<MenuItem>();
            TitleBackground = ConsoleColor.Blue;
            TitleForeground = ConsoleColor.Yellow;
            Titulo = titulo;
        }

        private int Select()
        {
            if (!Items[PosAtual].SubItemAberto)
            {
                Items[PosAtual].ShowSelector();
            } 
            while (true)
            {
                var tecla = Console.ReadKey();
                if (!Items[PosAtual].SubItemAberto)
                {
                    Items[PosAtual].Show();
                } else
                {
                    Items[PosAtual].SubItens[PosSubItem].Show();
                }
                if ((tecla.Modifiers & ConsoleModifiers.Alt) != 0)
                {
                    if (!Items[PosAtual].SubItemAberto)
                    {
                        for (int i = 0; i < Items.Count; i++)
                        {
                            if (Items[i].Rotulo.ToLower().Contains(char.ToLower(tecla.KeyChar)))
                            {
                                PosAtual = i;
                                Items[PosAtual].ShowSelector();
                                break;
                            }
                        }
                    } else {
                        for (int i = 0; i < Items[PosAtual].SubItens.Count; i++)
                        {
                            if (Items[PosAtual].SubItens[i].Rotulo.ToLower().Contains(char.ToLower(tecla.KeyChar)))
                            {
                                PosSubItem = i;
                                Items[PosSubItem].ShowSelector();
                                break;
                            }
                        }
                    }

                } else {
                    switch (tecla.Key)
                    {
                        case ConsoleKey.Enter:
                            if (Items[PosAtual].SubItens.Count == 0)
                            {
                                Items[PosAtual].ExecutarEventoClick();
                                break;
                            }

                            if (Items[PosAtual].SubItemAberto)
                            {
                                Items[PosAtual].SubItens[PosSubItem].ExecutarEventoClick();
                                break;
                            }

                            int x, y = 0;
                            y = (Console.WindowHeight - Items.Count - 2) / 2;
                            Items[PosAtual].SubItemAberto = true;
                            foreach (MenuItem s in Items[PosAtual].SubItens)
                            {
                                {
                                    x = (Console.WindowWidth - s.Rotulo.Length) / 2;
                                }

                                s.Lin = y++;
                                s.Col = x;
                                s.Show();
                            }
                            Console.SetCursorPosition(Items[PosAtual].SubItens[0].Col, Items[PosAtual].SubItens[0].Lin);
                            Console.BackgroundColor = Items[PosAtual].SelectorBackground;
                            Console.ForegroundColor = Items[PosAtual].SelectorForeground;
                            Console.Write(Items[PosAtual].SubItens[0].Rotulo);
                            Console.SetCursorPosition(Items[PosAtual].SubItens[0].Col, Items[PosAtual].SubItens[0].Lin);
                            break;
                        case ConsoleKey.Escape:
                            {
                                if (Items[PosAtual].SubItemAberto)
                                {
                                    Items[PosAtual].SubItemAberto = false;
                                    PosSubItem = 0;
                                    this.Show();
                                    break;  
                                }
                                Console.SetCursorPosition(0,0);
                                Console.Clear();
                                Console.WriteLine("Saindo...");
                                Environment.Exit(0);
                                break;
                            }
                        case ConsoleKey.RightArrow:
                            {
                                if (Items[PosAtual].SubItemAberto)
                                    break;
                                if (++PosAtual == Items.Count) PosAtual = 0;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            {
                                if (Items[PosAtual].SubItemAberto)
                                    break;
                                if (--PosAtual < 0) PosAtual = Items.Count - 1;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                if (!Items[PosAtual].SubItemAberto)
                                    break;
                                if (++PosSubItem == Items[PosAtual].SubItens.Count) PosSubItem = 0;
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            {
                                if (!Items[PosAtual].SubItemAberto)
                                    break;
                                if (--PosSubItem < 0) PosSubItem = Items[PosAtual].SubItens.Count - 1;
                            }
                            break;
                    }
                }
                if (!Items[PosAtual].SubItemAberto)
                {
                    Items[PosAtual].ShowSelector();
                } else
                {
                    Items[PosAtual].SubItens[PosSubItem].ShowSelector();
                }
                if (tecla.Key != ConsoleKey.Enter)
                {
                    EventoOpcaoTrocada?.Invoke(this, Items[PosAtual].Rotulo);
                }
            }
        }

        public void Show()
        {
            int x = 0, y = 0;
            PosAtual = 0;
            Console.Clear();
            if(Items.Count == 0)
            {
                throw new ArgumentException("Menu não contém items definidos.");
            }


            //mostra titulo
            Console.BackgroundColor = TitleBackground;
            Console.ForegroundColor = TitleForeground;
            Console.SetCursorPosition(x, y);
            Console.WriteLine(Titulo);
            Console.WriteLine();
            y += 2;
            foreach(MenuItem m in Items)
            {
                m.Lin = y;
                m.Col = x++;
                x += m.Rotulo.Length;
                m.Show();
            }
            Console.CursorVisible = false;
            var selected = Select();
            Console.CursorVisible = true;
        }

        
    }
}
