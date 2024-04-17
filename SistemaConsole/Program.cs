using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu("MENU");
            int ultimaLinha = Console.BufferHeight - 1;
            menu.Items.Add(new MenuItem
            {
                Rotulo = "A - Opção 1",
                Hint = new Hint("Primeira opção selecionada.", 0, ultimaLinha),
                SubItens = { new MenuItem { Rotulo = "Subitem 1", Hint = new Hint("Subitem 1 da opção 1 selecionado.", 0, ultimaLinha) },
                             new MenuItem { Rotulo = "Subitem 2", Hint = new Hint("Subitem 2 da opção 1 selecionado.", 0, ultimaLinha) } }
            });
            menu.Items.Add(new MenuItem
            {
                Rotulo = "B - Opção 2",
                Hint = new Hint("Segunda opção selecionada.", 0, ultimaLinha),
                SubItens = { new MenuItem { Rotulo = "Subitem 1", Hint = new Hint("Subitem 1 da opção 2 selecionado.", 0, ultimaLinha) },
                             new MenuItem { Rotulo = "Subitem 2", Hint = new Hint("Subitem 2 da opção 2 selecionado.", 0, ultimaLinha) } }
            });
            menu.Items.Add(new MenuItem
            {
                Rotulo = "C - Opção 3",
                Hint = new Hint("Terceira opção selecionada.", 30, ultimaLinha),
                SubItens = { new MenuItem { Rotulo = "Subitem 1", Hint = new Hint("Subitem 1 da opção 3 selecionado.", 0, ultimaLinha) },
                             new MenuItem { Rotulo = "Subitem 2", Hint = new Hint("Subitem 2 da opção 3 selecionado.", 0, ultimaLinha) },
                             new MenuItem { Rotulo = "Subitem 3", Hint = new Hint("Subitem 3 da opção 3 selecionado.", 0, ultimaLinha) }}
            });
            menu.Items.Add(new MenuItem
            {
                Rotulo = "D - Opção 4",
                Hint = new Hint("quarta opção selecionada.", 0, ultimaLinha),
            });

            menu.Items[3].EventoClick += EventoTesteClick;
            menu.Items[0].SubItens[0].EventoClick += EventoTesteClick;
            menu.EventoOpcaoTrocada += OpcaoAlteradaEvent;

            menu.Show();
            Console.ReadKey();
        }


        private static void OpcaoAlteradaEvent(object sender, string rotulo)
        {
            Console.Title = "Opção alterada para: " + rotulo;
        }



        private static void EventoTesteClick(object sender, string rotulo)
        {
            Console.Title = "Evento click opção: " + rotulo;
        }
    }
}
