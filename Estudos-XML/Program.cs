using System;
using System.Collections;
using System.Xml;

namespace Estudos.XML
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CriarXml();
            CriarXmlComTag();
            LerXml();
        }

        private static void CriarXml()
        {
            try
            {
                var writer = new XmlTextWriter(@"C:\TMP\filmes.xml", null);

                //inicia o documento xml
                writer.WriteStartDocument();
                //escreve o elmento raiz
                writer.WriteStartElement("filmes");
                //Escreve os sub-elementos
                writer.WriteElementString("titulo", "Cada & Companhia");
                writer.WriteElementString("titulo", "007 contra Godzila");
                writer.WriteElementString("titulo", "O segredo do Dr. Haus's");
                // encerra o elemento raiz
                writer.WriteEndElement();
                //Escreve o XML para o arquivo e fecha o objeto escritor
                writer.Close();
                Console.WriteLine("Arquivo XML gerado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void CriarXmlComTag()
        {
            try
            {
                var writer = new XmlTextWriter(@"C:\TMP\filmes2.xml", null);

                //inicia o documento xml
                writer.WriteStartDocument();

                //define a indentação do arquivo
                writer.Formatting = Formatting.Indented;
                //escreve um comentário
                writer.WriteComment("Arquivos de filmes");

                //escreve o elmento raiz
                writer.WriteStartElement("filmes");

                //escrever o atributo ano com valor 2007
                writer.WriteAttributeString("ano", "2012");

                //Escreve os sub-elementos
                writer.WriteElementString("titulo", "Cada & Companhia");
                writer.WriteElementString("titulo", "007 contra Godzila");
                writer.WriteElementString("titulo", "O segredo do Dr. Haus's");
                // encerra o elemento raiz
                writer.WriteEndElement();
                //Escreve o XML para o arquivo e fecha o objeto escritor
                writer.Close();
                Console.WriteLine("Arquivo XML gerado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void LerXml()
        {
            var reader = new XmlTextReader(@"C:\TMP\catalogo-livros.xml");

            var elementos = new ArrayList();

            while (reader.Read())
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.HasAttributes)
                            while (reader.MoveToNextAttribute())
                                elementos.Add(reader.Value);
                        break;
                    case XmlNodeType.Text:
                        //Incluir o texto do elemento no ArrayList
                        elementos.Add(reader.Value);
                        break;
                }

            //foreach(var num in elementos)
            //{
            //    lstXML.Items.Add(num);
            //}
        }
    }
}