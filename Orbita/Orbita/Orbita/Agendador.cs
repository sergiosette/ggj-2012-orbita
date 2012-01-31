using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Esfera
{
    class Agendador
    {
        static IDictionary <IAgendadorListener, Agendamento> listeners = new Dictionary<IAgendadorListener,Agendamento>();

        public Agendador()
        {

        }

        public static void AddListener (IAgendadorListener listener, int limiteMilisegudos)
        {
            listeners.Add(listener, new Agendamento(TimeSpan.FromMilliseconds(limiteMilisegudos)));
        }

        public void RemoveListener (IAgendadorListener listener)
        {
            listeners.Remove(listener);
        }


        public static void Update(GameTime time)
        {
            foreach (KeyValuePair<IAgendadorListener, Agendamento> par in listeners)
            {
                par.Value.Acumulo += time.ElapsedGameTime;
                if (par.Value.Acumulo > par.Value.Limite)
                {
                    par.Value.Acumulo -= par.Value.Limite;
                    par.Key.AgendamentoDisparado();
                }
            }
        }
    }

    class Agendamento
    {
        public TimeSpan Acumulo{get;set;}
        public TimeSpan Limite { get; set; }
        public Agendamento(TimeSpan limite)
        {
            Limite = limite;
            Acumulo = TimeSpan.FromTicks(0);
        }
    }


    interface IAgendadorListener
    {
        void AgendamentoDisparado();
    }
}
