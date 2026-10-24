using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MiniBanco.ConsoleApp.Models
{
    public enum TipoTransacao
    {
        Deposito,
        Saque,
        Tarifa,
        Transferencia,
        Rendimento
    }

    public class Transacao
    {
        public TipoTransacao Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }

        public Transacao(TipoTransacao tipo, decimal valor, string descricao = "")
        {
            Tipo = tipo;
            Valor = valor;
            Descricao = descricao;
            Data = DateTime.Now;
        }
    }
}
