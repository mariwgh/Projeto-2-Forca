﻿// 24140 Mariana Marietti da Costa
// 24153 Rafaelly Maria Nascimento da Silva


using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;

namespace apListaLigada
{
    public partial class FrmAlunos : Form
    {
        ListaDupla<Dicionario> lista1 = new ListaDupla<Dicionario>();
        int tempoRestante = 50;     //em segundos
        int pontos = 0;
        string caminho = null;
        int limiteSuperior = 0; // y mínimo que o enforcado pode subir


        public FrmAlunos()
        {
            InitializeComponent();

        }

        private void btnLerArquivo1_Click(object sender, EventArgs e)
        {

        }

        private void FazerLeitura(ref ListaDupla<Dicionario> qualLista)
        {
            // instanciar a lista de palavras e dicas
            ListaDupla<Dicionario> listaPalavraDica = new ListaDupla<Dicionario>();

            // pedir ao usuário o nome do arquivo de entrada
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecione um arquivo:";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                caminho = ofd.FileName;
            }

            // abrir esse arquivo e lê-lo linha a linha
            string linha = "";
            StreamReader arquivo = new StreamReader(caminho);
            while (!arquivo.EndOfStream)
            {
                linha = arquivo.ReadLine();

                // para cada linha, criar um objeto da classe de Palavra e Dica
                Dicionario palavraDica = new Dicionario(linha);

                // e inseri-lo no final da lista duplamente ligada
                listaPalavraDica.InserirAposFim(palavraDica);
            }

            qualLista = listaPalavraDica;

            arquivo.Close();
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            // se o usuário digitou palavra e dica:
            if (txtPalavra.Text != "" && txtDica.Text != "")
            {
                // criar objeto da classe Palavra e Dica para busca
                Dicionario palavraDica = new Dicionario(txtPalavra.Text, txtDica.Text);

                // tentar incluir em ordem esse objeto na lista1
                try
                {
                    lista1.InserirEmOrdem(palavraDica);
                    MessageBox.Show("Palavra inserida!");
                }

                // se não incluiu (já existe) avisar o usuário
                catch
                {
                    MessageBox.Show("Já existe.");
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // se a palavra digitada não é vazia:
            if (txtPalavra.Text != "")
            {
                // criar um objeto da classe de Palavra e Dica para busca
                Dicionario palavraDica = new Dicionario(txtPalavra.Text, "-");

                // se a palavra existe na lista1, posicionar o ponteiro atual nesse nó e exibir o registro atual
                if (lista1.Existe(palavraDica))
                {
                    NoDuplo<Dicionario> aux = lista1.Primeiro;         // ponteiro auxiliar para percorrer a lista que comeca com o primeiro no
                    int indiceBuscado = 0;

                    while (aux != null)
                    {
                        if (aux.Info.Palavra == palavraDica.Palavra)    // se achou, posiciona no indice que foi calculado
                        {
                            lista1.PosicionarEm(indiceBuscado);
                            break;
                        }
                        else
                        {
                            aux = aux.Prox;                             // avança para o próximo nó
                            indiceBuscado++;
                        }
                    }
                    
                    ExibirRegistroAtual();
                }

                // senão, avisar usuário que a palavra não existe
                else
                {
                    MessageBox.Show("A palavra não existe.");
                }

                // exibir o nó atual
                // ExibirRegistroAtual();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // para o nó atualmente visitado e exibido na tela:
            if (txtPalavra.Text != "")
            {
                // perguntar ao usuário se realmente deseja excluir essa palavra e dica
                DialogResult resposta = MessageBox.Show($"Deseja realmente excluir a palavra '{lista1.Atual.Info.Palavra.Trim()}'?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                // se sim, remover o nó atual da lista duplamente ligada e exibir o próximo nó
                if (resposta == DialogResult.Yes)
                {
                    lista1.Remover(new Dicionario(txtPalavra.Text, "-"));
                    MessageBox.Show("Excluído!");
                    
                    ExibirRegistroAtual();
                }
            }

            // se não, manter como está
        }

        private void FrmAlunos_FormClosing(object sender, FormClosingEventArgs e)
        {
            // solicitar ao usuário que escolha o arquivo de saída
            string caminhoSaida = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Selecione um arquivo de saída: ";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                caminhoSaida = openFileDialog.FileName;
            }

            // percorrer a lista ligada e gravar seus dados no arquivo de saída
            while (lista1.Atual != null)
            {
                lista1.GravarDados(caminhoSaida);
                lista1.Avancar();
            }
        }

        private void ExibirDados(ListaDupla<Aluno> aLista, ListBox lsb, Direcao qualDirecao)
        {
            lsb.Items.Clear();
            var dadosDaLista = aLista.Listagem(qualDirecao);
            foreach (Aluno aluno in dadosDaLista)
                lsb.Items.Add(aluno);
        }

        private void tabControl1_Enter(object sender, EventArgs e)
        {
            rbFrente.PerformClick();
        }

        private void rbFrente_Click(object sender, EventArgs e)
        {
            //ExibirDados(lista1, lsbDados, Direcao.paraFrente);
        }

        private void rbTras_Click(object sender, EventArgs e)
        {
            //ExibirDados(lista1, lsbDados, Direcao.paraTras);
        }

        private void FrmAlunos_Load(object sender, EventArgs e)
        {
            // fazer a leitura do arquivo escolhido pelo usuário e armazená-lo na lista1
            FazerLeitura(ref lista1);

            // posicionar o ponteiro atual no início da lista duplamente ligada
            lista1.PosicionarNoInicio();

            // Exibir o Registro Atual;
            ExibirRegistroAtual();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            // posicionar o ponteiro atual no início da lista duplamente ligada
            lista1.PosicionarNoInicio();

            // Exibir o Registro Atual;
            ExibirRegistroAtual();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            // Retroceder o ponteiro atual para o nó imediatamente anterior 
            lista1.Retroceder();

            // Exibir o Registro Atual;
            ExibirRegistroAtual();
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            // Retroceder o ponteiro atual para o nó seguinte 
            lista1.Avancar();

            // Exibir o Registro Atual;
            ExibirRegistroAtual();
        }

        private void btnFim_Click(object sender, EventArgs e)
        {
            // posicionar o ponteiro atual no último nó da lista 
            lista1.PosicionarNoFinal();

            // Exibir o Registro Atual;
            ExibirRegistroAtual();
        }

        private void ExibirRegistroAtual()
        {
            // se a lista não está vazia:
            if (!lista1.EstaVazia)
            {
                // acessar o nó atual e exibir seus campos em txtDica e txtPalavra
                var noAtual = lista1[lista1.NumeroDoNoAtual];
                txtPalavra.Text = noAtual.Palavra;
                txtDica.Text = noAtual.Dica;

                // atualizar no status bar o número do registro atual / quantos nós na lista
                slRegistro.Text = $"Registro: {lista1.NumeroDoNoAtual + 1}/{lista1.QuantosNos}";
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // alterar a dica e guardar seu novo valor no nó exibido
            if (!lista1.EstaVazia)
            {
                var registroAtual = lista1[lista1.NumeroDoNoAtual];

                if (registroAtual != null)
                {
                    registroAtual.Dica = txtDica.Text;
                    MessageBox.Show("Dica alterada!");
                }
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void txtPalavra_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // cria uma instância de Random para gerar números aleatórios
            Random random = new Random();

            // sorteio do número do elemento (posição) que será acessado na lista
            int posicaoSorteada = random.Next(lista1.QuantosNos);

            // posiciona o ponteiro atual na posição sorteada
            lista1.PosicionarEm(posicaoSorteada);

            // chama o evento para ocultar a aba de cadastro
            tabControl1_VisibleChanged(sender, e);

            // limpa o DataGridView
            dataGridView.Columns.Clear();

            // configura para que o numero de colunas seja igual ao número de caracteres (sem os espaços à direita) da palavra lida do arquivo.
            dataGridView.ColumnCount = lista1.Atual.Info.Palavra.ToUpper().Trim().Length;

            if (checkDica.Checked)
            {
                // exibe a dica da palavra sorteada lida noS label [Dica:].
                labelDica.Text = $"{lista1.Atual.Info.Dica}";
                tmrTempo.Enabled = true;
                tmrTempo.Start();
                tempoRestante = 5000;
                tmrTempo.Interval = 1000;
                labelTempo.Text = $"Tempo: {tempoRestante}s";
            }

            if (checkArduino.Checked) {
                ArduinoVisivel();
            }
        }

        private void tabControl1_VisibleChanged(object sender, EventArgs e)
        {
            // verifica se a aba de cadastro já está visível ou não
            // se estiver visível, remove a aba; se não estiver, adiciona a aba
            if (tabControl1.Contains(tpCadastro))
            {
                // remover a aba de cadastro
                tabControl1.TabPages.Remove(tpCadastro);
            }
            else
            {
                // adicionar a aba de cadastro
                tabControl1.TabPages.Add(tpCadastro);
            }    
        }

        private void button27_Click(object sender, EventArgs e)
        {
            // pegar letra clicada
            string letra = (sender as Button).Text;
            string caractereAtual = "";
            int erros = 0;
            int indicePalavra = 0;

            string palavra = lista1.Atual.Info.Palavra.ToUpper().Trim();
            string dica = lista1.Atual.Info.Dica.ToUpper().Trim();

            Dicionario vetor = lista1.Atual.Info;
            
            // verificar se a letra existe na palavra sorteada
            if (lista1.Atual.Info.ExisteLetra(letra)) 
            {
                pontos++;

                for (int i = 0; i <= palavra.Length; i++)
                {
                    if (caractereAtual != letra)
                    {
                        caractereAtual = palavra.Substring(i, 1);
                        
                        //precisaria colocar um vetor.acertou[i] = false, mas a classe ja inicializa com false em todas as posições
                    }
                    else
                    {
                        //se sim, exibir a letra no dataGridView no local indicado
                        ExibirDGV(0, i, caractereAtual);
                        indicePalavra++;
                        //marcar com true essa mesma posição no vetor acertou do objeto
                        vetor.acertou[i] = true; ;   //TROCAR TODOS OS ACERTOU POR LÓGICA LISTA -> NÃO SEI COMO FAZER ISSO
                        //PQ A acertou É LIST E N LISTA LIGADA MSM AAAAAAAAAAAAAAAAA
                    }
                }
            }

            //se não, colocar a proxima foto do homem enforcado
            else
            {
                erros++;
                pontos--;

                switch (erros) {
                    case 1: { pict1.Visible = true; break; }
                    case 2: { pict2.Visible = true; break; }
                    case 3: { pict3.Visible = true; break; }
                    case 4: { pict4.Visible = true; break; }
                    case 5: { pict5.Visible = true; break; }
                    case 6: { pict6.Visible = true; break; }
                    case 7: { pict7.Visible = true; break; }
                    case 8: { 
                            
                            Perdeu();  //--> precisa achar um jeito de parar o jogo, mas como??
                            break; 
                    }

                    default: { throw new Exception("Valor de erros fora do previsto."); }
                }
            }

            labelPontos.Text = $"{pontos}";
            labelErros.Text = $"{erros}";

            //oq essa parte faz?
            bool dadoAtual = true;
            //int contador = 0;

            if (indicePalavra == palavra.Length) {
                while (dadoAtual)
                {
                    for (int i = 0; i < palavra.Length; i++)
                    {
                        dadoAtual = vetor.acertou[i];
                    }
                }
                if (dadoAtual)
                {
                    Ganhou();
                }
                else
                {
                    Perdeu();
                }
            }
        }

        public void ExibirDGV(int linha, int coluna, string letra)
        {
            dataGridView.Rows[linha].Cells[coluna].Value = letra;
        }

        //a cada segundo ele chama essa funcao, logo, diminui a contagem do tempo restante a cada 1 segundo e exibe,
        //verificando se o tempo acabou ou nao 
        private void tmrTempo_Tick(object sender, EventArgs e)
        {
            tempoRestante--;
            labelTempo.Text = $"{tempoRestante}s";

            //se o tempo acabou
            if (tempoRestante <= 0)
            {
                tmrTempo.Stop();

                //tempo acabou e ele n ganhou, perdeu
                if (!Ganhou()) 
                {
                    Perdeu();
                }

                int alturaFinal = -100;     // para onde a imagem deve subir
                int velocidadeSubida = 5;   // velocidade da ascensão
                                            // Mover o PictureBox para cima
                pictEnforcado1.Top -= velocidadeSubida;
                pictEnforcado2.Top -= velocidadeSubida;
            }
        }

        public bool Ganhou()
        {
            bool venceu = false;
            if (pontos == lista1.Atual.Info.Palavra.Trim().Length)
            {
                pictGanhou1.Visible = true;
                pictGanhou2.Visible = true;
                pictGanhou3.Visible = true;
                venceu = true;
            }
            return venceu;
        }

        public bool Perdeu()
        {
            bool derrota = false;
            if (pontos != lista1.Atual.Info.Palavra.Trim().Length)
            {
                pict8.Visible = true;

                pictEnforcado1.Visible = true;
                limiteSuperior = pictEnforcado1.Top - 150;
                tmrEnforcado.Start();

                pictEnforcado2.Visible = true;

                derrota = true;
            }
               
            //fzr a animação aqui
           
            return derrota;
        }

        public void ArduinoVisivel()
        {
            labelArduino.Visible = true;
            txtPortaArduino.Visible = true;
        }

        private void tmrEnforcado_Tick(object sender, EventArgs e)
        {
            pictEnforcado1.Top -= 5;  // sobe 5 pixels por tick

            if (pictEnforcado1.Top <= limiteSuperior)
            {
                tmrEnforcado.Stop();
                MessageBox.Show("Ele ascendeu aos céus...");
            }
        }
    }
}
