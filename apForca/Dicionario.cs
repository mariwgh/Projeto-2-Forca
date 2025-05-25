// 24140 Mariana Marietti da Costa
// 24153 Rafaelly Maria Nascimento da Silva


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apListaLigada
{
    public class Dicionario : IComparable<Dicionario>, IRegistro,
                      ICriterioDeSeparacao<Dicionario>
    {
        // atributos da antiga classe PalavraDica, agora Dicionario:
        string palavra, dica;
        private const int tamanhoMaximo = 30;

        // propriedades e metodos projeto II:
            // propriedade de acesso
        bool acertou;   // vetor de valores lógicos que sempre será iniciado com 15 posições valendo falso

        public void Acertou()
        {
            // retornar true se a letra sorteada está na palavra
            // atualizando o vetor acertou com true nas posições onde a encontrou;
            acertou = true;
        }

        // projeto I
        public string Palavra
        {
            get => palavra;
            set
            {
                // se houver palavra, atribui o valor
                if (value != "")
                    // se for menor, preenche com espaços, e se a palavra for maior que o tamanho máximo, corta
                    palavra = value.PadRight(tamanhoMaximo, ' ').Substring(0, tamanhoMaximo);
                else
                    throw new Exception("Palavra vazia é inválido.");
            }
        }
        public string Dica
        {
            get => dica;
            set
            {
                if (value != null)
                    dica = value;
                else
                    throw new Exception("Dica vazia é inválido.");
            }
        }

        // construtores da classe PalavraDica: 
        // se passar a palavra e a dica ja separadas
        public Dicionario(string palavra, string dica)
        {
            // se a palavra for maior que o tamanho máximo, lança uma exceção
            if (palavra.Length > tamanhoMaximo)
            {
                throw new Exception("30 caracteres atingidos.");
            }

            Palavra = palavra;
            Dica = dica;
        }

        // se passar a palavra e a dica juntas
        public Dicionario(string linhaDeDados)
        {
            // separa a palavra, que é do 0 ate 30
            Palavra = linhaDeDados.PadRight(tamanhoMaximo, ' ').Substring(0, tamanhoMaximo);

            // se houver dica, ou seja, houver mais que 30 caracteres
            if (linhaDeDados.Length > tamanhoMaximo)
            {
                // separa a dica, que é do 30 até o final
                Dica = linhaDeDados.Substring(tamanhoMaximo).Trim();
            }
            // se não houver dica, atribui uma string vazia
            else
            {
                Dica = "";
            }
        }


        public int CompareTo(Dicionario other)
        {
            return this.palavra.CompareTo(other.palavra);
        }

        public bool DeveSeparar()
        {
            throw new NotImplementedException();
        }

        public string FormatoDeArquivo()
        {
            return $"{palavra}{dica}";
        }

        public override string ToString()
        {
            return palavra + " " + dica;
        }
    }
}
