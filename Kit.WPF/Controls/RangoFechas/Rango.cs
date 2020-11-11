﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kit.WPF.Controls.RangoFechas
{
    public class Rango : ViewModelBase<Rango>
    {
        private DateTime? _Inicio;
        public DateTime? Inicio
        {
            get => _Inicio;
            set
            {
                if (_Inicio != value)
                {
                    _Inicio = value;
                    OnPropertyChanged();
                    OnDateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private DateTime? _Fin;
        public DateTime? Fin
        {
            get => _Fin;
            set
            {
                if (_Fin != value)
                {
                    _Fin = value;
                    OnPropertyChanged();
                    OnDateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _TodasLasFechas;
        public bool TodasLasFechas
        {
            get => _TodasLasFechas;
            set
            {
                _TodasLasFechas = value;
                OnPropertyChanged();
                OnDateChanged?.Invoke(this, EventArgs.Empty);
                //if (TodasLasFechas)
                //{
                //    Fin =
                //    Inicio = null;
                //}
                //else
                //{
                //    Inicio = DateTime.Now;
                //    Fin = DateTime.Now;
                //}
            }
        }
        public bool Cancelado { get; set; }
        public bool Excel { get; set; }
        public event EventHandler OnDateChanged;
        public Rango()
        {
            Excel = false;
            Cancelado = false;
            _Inicio = DateTime.Now;
            _Fin = DateTime.Now;
        }
    }
}