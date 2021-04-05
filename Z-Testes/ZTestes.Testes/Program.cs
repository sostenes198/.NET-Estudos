using Aspose.Pdf;

namespace ZTestes.Testes  
{
    class Program  
    {  
        static void Main(string[] args)
        {
            var path = "Formulário Benefícios_Preenchido.jpeg";

            Document doc = new Document();

            Page page = doc.Pages.Add();
            var image = new Image();
            image.File = (path);

            page.Paragraphs.Add(image);

            doc.Save("Formulário Benefícios_Preenchido.pdf");
        }  
    }
}  