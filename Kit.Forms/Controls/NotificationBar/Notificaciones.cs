﻿
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kit.Forms.Controls.NotificationBar
{
    public class Notificaciones : ViewModelBase<Notificaciones>
    {
        private static Notificaciones _Instance;
        public static Notificaciones Instance
        {
            get => _Instance;
            set
            {
                _Instance = value;
                OnGlobalPropertyChanged();
            }
        }
        private string _Color;
        public string Color
        {
            get => _Color;
            set
            {
                _Color = value;
                Raise(() => this.Color);
            }
        }
        public ObservableCollection<Notificacion> Elementos { get; private set; }
        public static Notificaciones Init(string Color = "#37FFFFFF")
        {
            NotificationBar.Notificaciones.Instance = new NotificationBar.Notificaciones(Color);
            return Instance;
        }
        public Notificaciones(string Color = "#37FFFFFF")
        {
            this.Color = Color;
            this.Elementos = new ObservableCollection<Notificacion>();
        }
        ~Notificaciones()
        {

        }
        private async Task Notificar(Notificacion Notificacion)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                this.Elementos.Add(Notificacion);
            });
        }
        private async Task Remove(Notificacion notificacion)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                this.Elementos.Remove(notificacion);
            });
        }
    }
}
