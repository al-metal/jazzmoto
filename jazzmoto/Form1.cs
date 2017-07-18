using Bike18;
using RacerMotors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet.Net;
using Формирование_ЧПУ;

namespace jazzmoto
{
    public partial class Form1 : Form
    {
        Thread forms;

        string minitextTemplate;
        string fullTextTemplate;
        string keywordsTextTemplate;
        string titleTextTemplate;
        string descriptionTextTemplate;
        string otv;
        string boldOpen = "<span style=\"\"font-weight: bold; font-weight: bold;\"\">";
        string boldClose = "</span>";
        int countUpdateImage = 0;

        bool chekedSEO = false;
        bool chekedMiniText = false;

        List<string> newProduct = new List<string>();
        FileEdit files = new FileEdit();
        CHPU chpu = new CHPU();
        CookieContainer cookieNethouse = new CookieContainer();
        nethouse nethouse = new nethouse();
        CookieContainer cookie = new CookieContainer();
        WebClient webClient = new WebClient();

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists("files"))
            {
                Directory.CreateDirectory("files");
            }
            if (!Directory.Exists("pic"))
            {
                Directory.CreateDirectory("pic");
            }

            if (!File.Exists("files\\miniText.txt"))
            {
                File.Create("files\\miniText.txt");
            }

            if (!File.Exists("files\\fullText.txt"))
            {
                File.Create("files\\fullText.txt");
            }

            if (!File.Exists("files\\title.txt"))
            {
                File.Create("files\\title.txt");
            }

            if (!File.Exists("files\\description.txt"))
            {
                File.Create("files\\description.txt");
            }

            if (!File.Exists("files\\keywords.txt"))
            {
                File.Create("files\\keywords.txt");
            }
            StreamReader altText = new StreamReader("files\\miniText.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                rtbMiniText.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\fullText.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                rtbFullText.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\title.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                tbTitle.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\description.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                tbDescription.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\keywords.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                tbKeywords.AppendText(str + "\n");
            }
            altText.Close();
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            int count = 0;
            StreamWriter writers = new StreamWriter("files\\miniText.txt", false, Encoding.GetEncoding(1251));
            count = rtbMiniText.Lines.Length;
            for (int i = 0; rtbMiniText.Lines.Length > i; i++)
            {
                if (count - 1 == i)
                {
                    if (rtbFullText.Lines[i] == "")
                        break;
                }
                writers.WriteLine(rtbMiniText.Lines[i].ToString());
            }
            writers.Close();

            writers = new StreamWriter("files\\fullText.txt", false, Encoding.GetEncoding(1251));
            count = rtbFullText.Lines.Length;
            for (int i = 0; count > i; i++)
            {
                if (count - 1 == i)
                {
                    if (rtbFullText.Lines[i] == "")
                        break;
                }
                writers.WriteLine(rtbFullText.Lines[i].ToString());
            }
            writers.Close();

            writers = new StreamWriter("files\\title.txt", false, Encoding.GetEncoding(1251));
            writers.WriteLine(tbTitle.Lines[0]);
            writers.Close();

            writers = new StreamWriter("files\\description.txt", false, Encoding.GetEncoding(1251));
            writers.WriteLine(tbDescription.Lines[0]);
            writers.Close();

            writers = new StreamWriter("files\\keywords.txt", false, Encoding.GetEncoding(1251));
            writers.WriteLine(tbKeywords.Lines[0]);
            writers.Close();

            MessageBox.Show("Сохранено");
        }

        private void btnActual_Click(object sender, EventArgs e)
        {
            #region Сохранение паролей
            Properties.Settings.Default.login = tbLoginNethouse.Text;
            Properties.Settings.Default.password = tbPassNethouse.Text;
            Properties.Settings.Default.Save();
            #endregion

            #region Обработка сайта

            minitextTemplate = MinitextStr();
            fullTextTemplate = FulltextStr();
            keywordsTextTemplate = tbKeywords.Lines[0].ToString();
            titleTextTemplate = tbTitle.Lines[0].ToString();
            descriptionTextTemplate = tbDescription.Lines[0].ToString();

            Thread tabl = new Thread(() => ActualJazzMoto());
            forms = tabl;
            forms.IsBackground = true;
            forms.Start();

            #endregion
        }

        private void ActualJazzMoto()
        {
            ControlsFormEnabledFalse();
            cookieNethouse = nethouse.CookieNethouse(tbLoginNethouse.Text, tbPassNethouse.Text);

            if (cookieNethouse.Count == 1)
            {
                MessageBox.Show("Логин или пароль для сайта Nethouse введены не верно", "Ошибка логина/пароля");
                ControlsFormEnabledTrue();
                return;
            }

            chekedSEO = cbSEO.Checked;
            chekedMiniText = cbMinitext.Checked;

            File.Delete("allTovars");
            File.Delete("naSite.csv");
            newProduct = newList();
            

            string otv = GetRequest("https://jazzmoto.ru/shop/tovari/dlya_pitbaykov/"); 

            string textAllCategory = new Regex("(?<=<li id=\")[\\w\\W]*?(?=</li></ul>)").Match(otv).ToString();
            MatchCollection urlNameCategory = new Regex("(?<=\"><a).*</a> <span>").Matches(textAllCategory);
            for (int i = 0; urlNameCategory.Count > i; i++)
            {
                string urlCategory = new Regex("(?<=href=\").*(?=\">)").Match(urlNameCategory[i].ToString()).ToString();
                urlCategory = "https://jazzmoto.ru" + urlCategory;
                string nameCategory = new Regex("(?<=\">).*(?=</a>)").Match(urlNameCategory[i].ToString()).ToString();
                GetUpdateTovar(cookieNethouse, urlCategory, urlNameCategory);
                UploadTovar();
                DeleteTovar();
            }

            ControlsFormEnabledTrue();
        }

        private void DeleteTovar()
        {
            otv = GetRequest("https://bike18.ru/products/category/zapchasti-dlya-pitbikov");
            MatchCollection razdelSite = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
            string[] allprod = File.ReadAllLines("allTovars");
            for (int i = 0; razdelSite.Count > i; i++)
            {
                otv = GetRequest("https://bike18.ru" + razdelSite[i].ToString() + "?page=all");
                MatchCollection product = new Regex("(?<=<a href=\").*(?=\"><div class=\"-relative item-image\")").Matches(otv);
                for (int n = 0; product.Count > n; n++)
                {
                    string urlTovar = product[n].ToString();
                    otv = GetRequest(urlTovar);

                    if (otv == "err")
                    {
                        StreamWriter s = new StreamWriter("badURL.txt", true);
                        s.WriteLine(urlTovar);
                        s.Close();
                        continue;
                    }

                    string artProd = new Regex("(?<=Артикул:)[\\w\\W]*?(?=</div)").Match(otv).ToString().Trim();
                    bool b = false;

                    foreach (string str in allprod)
                    {
                        if (artProd == str)
                        {
                            b = true;
                            break;
                        }
                    }

                    if (!b)
                    {
                        nethouse.DeleteProduct(cookie, urlTovar);
                    }
                }
            }
        }

        private void UploadTovar()
        {
            cookie = nethouse.CookieNethouse(tbLoginNethouse.Text, tbPassNethouse.Text);
            System.Threading.Thread.Sleep(10000);
            string[] naSite1 = File.ReadAllLines("naSite.csv", Encoding.GetEncoding(1251));
            if (naSite1.Length > 1)
                nethouse.UploadCSVNethouse(cookie, "naSite.csv");
            File.Delete("naSite.csv");
            newProduct = newList();
        }

        private void WriteArticlInFile(string articl)
        {
            string article = articl.ToString();
            StreamWriter sw = new StreamWriter("allTovars", true);
            sw.WriteLine(article);
            sw.Close();
        }

        private string GetRequest(string v)
        {
            var request2 = new HttpRequest();
            request2.UserAgent = HttpHelper.RandomChromeUserAgent();
            request2.Proxy = HttpProxyClient.Parse("127.0.0.1:8888");
            // Отправляем запрос.
            HttpResponse response = request2.Get(v);
            // Принимаем тело сообщения в виде строки.
            string content = response.ToText();
            return content;
        }

        private void GetUpdateTovar(CookieContainer cookieNethouse, string urlCategory, MatchCollection urlNameCategory)
        {
            string otv = GetRequest(urlCategory + "?count=all");
            string allTovarsStr = new Regex("(?<=<div class=\"bx_catalog_list_home col3 bx_green\">)[\\w\\W]*?(?=id=\"add_ajax_item)").Match(otv).ToString();
            MatchCollection urlTovars = new Regex("(?<=<a href=\").*?(?=\" title=\")").Matches(allTovarsStr);
            foreach (Match s in urlTovars)
            {
                string urlTovar = "https://jazzmoto.ru" + s.ToString();

                List<string> tovarJMC = GetTovarJMC(urlTovar);

                if (tovarJMC == null)
                    continue;

                WriteArticlInFile(tovarJMC[0]);

                string urlTovarB18 = SearchTovar(tovarJMC);
                if (urlTovarB18 == "")
                {
                    GetWriteInCSV(tovarJMC);
                }
                else
                {
                    List<string> tovarB18 = nethouse.GetProductList(cookieNethouse, urlTovarB18);

                    string article = tovarJMC[0];
                    string newArticle = tovarJMC[1];
                    string name = tovarJMC[2];

                    bool edits = false;

                    if(tovarB18[9] != tovarJMC[3])
                    {
                        tovarB18[9] = tovarJMC[3];
                        edits = true;
                    }

                    if (chekedSEO)
                    {
                        string descriptionText = descriptionTextTemplate;
                        string titleText = titleTextTemplate;
                        string keywordsText = keywordsTextTemplate;

                        titleText = ReplaceSEO("title", titleText, name, article.Replace(";", " "), newArticle.Replace(";", " "));
                        descriptionText = ReplaceSEO("description", descriptionText, name, article, newArticle);
                        keywordsText = ReplaceSEO("keywords", keywordsText, name, article, newArticle);

                        tovarB18[11] = descriptionText;
                        tovarB18[12] = keywordsText;
                        tovarB18[13] = titleText;
                        edits = true;
                    }

                    if (chekedMiniText)
                    {
                        string miniDescription = tovarJMC[5];
                        string minitext = "<p>" + miniDescription + "</p>" + minitextTemplate;
                        minitext = Replace(minitext, name, article);
                        minitext = minitext.Remove(minitext.LastIndexOf("<p>"));
                        minitext = minitext.Replace("&#40;", "(").Replace("&#41;", ")");
                        tovarB18[7] = minitext;
                        
                        edits = true;
                    }

                    if (tovarB18[42].Contains("&alsoBuy[0]=0"))
                    {
                        tovarB18[42] = nethouse.alsoBuyTovars(tovarB18);
                        edits = true;
                    }

                    if (edits)
                        nethouse.SaveTovar(cookieNethouse, tovarB18);
                }
            }
        }

        private void GetWriteInCSV(List<string> tovarJMC)
        {
            string article = tovarJMC[0];
            string newArticle = tovarJMC[1];
            string name = tovarJMC[2];
            string price = tovarJMC[3];
            string razdel = tovarJMC[4];
            string miniDescription = tovarJMC[5];
            string slug = tovarJMC[6];

            string minitext = "<p>" + miniDescription + "</p>" + minitextTemplate;
            string fullText = fullTextTemplate;
            string descriptionText = descriptionTextTemplate;
            string titleText = titleTextTemplate;
            string keywordsText = keywordsTextTemplate;

            titleText = ReplaceSEO("title", titleText, name, article.Replace(";", " "), newArticle.Replace(";", " "));
            descriptionText = ReplaceSEO("description", descriptionText, name, article, newArticle);
            keywordsText = ReplaceSEO("keywords", keywordsText, name, article, newArticle);

            minitext = Replace(minitext, name, article);
            minitext = minitext.Remove(minitext.LastIndexOf("<p>"));

            fullText = Replace(fullText, name, article);
            fullText = fullText.Remove(fullText.LastIndexOf("<p>"));

            newProduct = new List<string>();
            newProduct.Add(""); //id
            newProduct.Add("\"" + newArticle + "\""); //артикул
            newProduct.Add("\"" + name + "\"");  //название
            newProduct.Add("\"" + price + "\""); //стоимость
            newProduct.Add("\"" + "" + "\""); //со скидкой
            newProduct.Add("\"" + razdel + "\""); //раздел товара
            newProduct.Add("\"" + "100" + "\""); //в наличии
            newProduct.Add("\"" + "0" + "\"");//поставка
            newProduct.Add("\"" + "1" + "\"");//срок поставки
            newProduct.Add("\"" + minitext + "\"");//краткий текст
            newProduct.Add("\"" + fullText + "\"");//полностью текст
            newProduct.Add("\"" + titleText + "\""); //заголовок страницы
            newProduct.Add("\"" + descriptionText + "\""); //описание
            newProduct.Add("\"" + keywordsText + "\"");//ключевые слова
            newProduct.Add("\"" + slug + "\""); //ЧПУ
            newProduct.Add(""); //с этим товаром покупают
            newProduct.Add("");   //рекламные метки
            newProduct.Add("\"" + "1" + "\"");  //показывать
            newProduct.Add("\"" + "0" + "\""); //удалить

            files.fileWriterCSV(newProduct, "naSite");
        }

        private string SearchTovar(List<string> tovarJMC)
        {
            string urlTovar = "";

            string name = tovarJMC[2];
            string article = tovarJMC[1];

            string otv = GetRequest("https://bike18.ru/products/search?sort=0&balance=&categoryId=&min_cost=&max_cost=&page=1&text=" + article);
            urlTovar = new Regex("(?<=<a href=\").*(?=\"><div class=\"-relative item-image\")").Match(otv).ToString();

            return urlTovar;
        }

        private List<string> GetTovarJMC(string urlTovar)
        {
            List<string> tovar = new List<string>();

            string otvTovar = GetRequest(urlTovar);
            bool availability = otvTovar.Contains("<span class=\"item_section_name_gray\" style=\"display: ;\">");

            if (!availability)
                return tovar = null;

            MatchCollection infoSection = new Regex("(?<=<div class=\"item_info_section\">)[\\w\\W]*?(?=</div>)").Matches(otvTovar);

            MatchCollection parametrsString = new Regex("(?<=<b>).*?(?=<br>)").Matches(infoSection[0].ToString());
            string article = new Regex("(?<=Товара:</b>).*?(?=<br>)").Match(infoSection[0].ToString()).ToString().Trim();
            if (article == "")
                return tovar = null;
            string newArticle = "JMC_" + article;

            string urlImg = new Regex("(?<=<img itemprop=\"image\").*(?=\" alt)").Match(otvTovar).ToString();
            urlImg = "https:" + new Regex("(?<=src=\").*").Match(urlImg).ToString();

            DownloadImages(urlImg, newArticle);

            string miniDescription = "";
            if (infoSection.Count == 5)
            {
                foreach (Match s in parametrsString)
                {
                    string parametrsStr = s.ToString().Replace(":</b>", "");
                    if (parametrsStr.Contains("ШтрихКод"))
                        continue;
                    miniDescription += parametrsStr + "<br >";
                }
                miniDescription = miniDescription.Replace(article, newArticle);
                miniDescription += infoSection[1].ToString();
            }
            miniDescription = miniDescription.Replace("\n", "");

            string price = "";
            if (infoSection.Count == 5)
                price = new Regex("(?<=Цена).*?(?=руб)").Match(infoSection[2].ToString()).ToString().Trim();
            else
                price = new Regex("(?<=Цена).*?(?=руб)").Match(infoSection[1].ToString()).ToString().Trim();
            price = ReturnPrice(price);
            string name = new Regex("(?<=<h1><span itemprop=\"name\">).*?(?=</span></h1>)").Match(otvTovar).ToString();
            name = name.Replace("/", "-");
            string slug = chpu.vozvr(name);

            string razdel = ReturnRazdel(otvTovar);

            tovar.Add(article);
            tovar.Add(newArticle);
            tovar.Add(name);
            tovar.Add(price);
            tovar.Add(razdel);
            tovar.Add(miniDescription);
            tovar.Add(slug);

            return tovar;
        }

        private string ReturnRazdel(string otvTovar)
        {
            string razdel = "Запчасти и расходники => Запчасти для питбайков => ";

            MatchCollection allRazdels = new Regex("(?<=<a href=\").*?(?=itemprop=\"url\">)").Matches(otvTovar);
            string nameRazdel = new Regex("(?<=title=\").*?(?=\")").Match(allRazdels[allRazdels.Count - 1].ToString()).ToString();

            razdel += nameRazdel;
            return razdel;
        }

        private void DownloadImages(string urlImg, object article)
        {
            if (!File.Exists("Pic\\" + article + ".jpg"))
            {
                try
                {
                    webClient.DownloadFile(urlImg, "Pic\\" + article + ".jpg");
                }
                catch
                {

                }
            }
        }

        private string ReturnPrice(string price)
        {
            price = price.Replace(" ", "");
            int p = Convert.ToInt32(price);
            p = p - (p / 100);
            p = (p / 10) * 10;
            price = p.ToString();
            return price;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbLoginNethouse.Text = Properties.Settings.Default.login;
            tbPassNethouse.Text = Properties.Settings.Default.password;
        }

        private string MinitextStr()
        {
            string minitext = "";
            for (int z = 0; rtbMiniText.Lines.Length > z; z++)
            {
                if (rtbMiniText.Lines[z].ToString() == "")
                {
                    minitext += "<p><br /></p>";
                }
                else
                {
                    minitext += "<p>" + rtbMiniText.Lines[z].ToString() + "</p>";
                }
            }
            return minitext;
        }

        private string FulltextStr()
        {
            string fullText = "";
            for (int z = 0; rtbFullText.Lines.Length > z; z++)
            {
                if (rtbFullText.Lines[z].ToString() == "")
                {
                    fullText += "<p><br /></p>";
                }
                else
                {
                    fullText += "<p>" + rtbFullText.Lines[z].ToString() + "</p>";
                }
            }
            return fullText;
        }

        private List<string> newList()
        {
            List<string> newProduct = new List<string>();
            newProduct.Add("id");                                                                               //id
            newProduct.Add("Артикул *");                                                 //артикул
            newProduct.Add("Название товара *");                                          //название
            newProduct.Add("Стоимость товара *");                                    //стоимость
            newProduct.Add("Стоимость со скидкой");                                       //со скидкой
            newProduct.Add("Раздел товара *");                                         //раздел товара
            newProduct.Add("Товар в наличии *");                                                    //в наличии
            newProduct.Add("Поставка под заказ *");                                                 //поставка
            newProduct.Add("Срок поставки (дни) *");                                           //срок поставки
            newProduct.Add("Краткий текст");                                 //краткий текст
            newProduct.Add("Текст полностью");                                          //полностью текст
            newProduct.Add("Заголовок страницы (title)");                               //заголовок страницы
            newProduct.Add("Описание страницы (description)");                                 //описание
            newProduct.Add("Ключевые слова страницы (keywords)");                                 //ключевые слова
            newProduct.Add("ЧПУ страницы (slug)");                                   //ЧПУ
            newProduct.Add("С этим товаром покупают");                              //с этим товаром покупают
            newProduct.Add("Рекламные метки");
            newProduct.Add("Показывать на сайте *");                                           //показывать
            newProduct.Add("Удалить *");                                    //удалить
            files.fileWriterCSV(newProduct, "naSite");
            return newProduct;
        }

        private void ControlsFormEnabledFalse()
        {
            btnActual.Invoke(new Action(() => btnActual.Enabled = false));
            btnImages.Invoke(new Action(() => btnImages.Enabled = false));
            btnSaveTemplate.Invoke(new Action(() => btnSaveTemplate.Enabled = false));
            rtbFullText.Invoke(new Action(() => rtbFullText.Enabled = false));
            rtbMiniText.Invoke(new Action(() => rtbMiniText.Enabled = false));
            tbDescription.Invoke(new Action(() => tbDescription.Enabled = false));
            tbKeywords.Invoke(new Action(() => tbKeywords.Enabled = false));
            tbTitle.Invoke(new Action(() => tbTitle.Enabled = false));
            tbLoginNethouse.Invoke(new Action(() => tbLoginNethouse.Enabled = false));
            tbPassNethouse.Invoke(new Action(() => tbPassNethouse.Enabled = false));
            cbMinitext.Invoke(new Action(() => cbMinitext.Enabled = false));
            cbSEO.Invoke(new Action(() => cbSEO.Enabled = false));
            gpOther.Invoke(new Action(() => gpOther.Enabled = false));
            cbOther.Invoke(new Action(() => cbOther.Enabled = false));
        }

        private void ControlsFormEnabledTrue()
        {
            btnActual.Invoke(new Action(() => btnActual.Enabled = true));
            btnImages.Invoke(new Action(() => btnImages.Enabled = true));
            btnSaveTemplate.Invoke(new Action(() => btnSaveTemplate.Enabled = true));
            rtbFullText.Invoke(new Action(() => rtbFullText.Enabled = true));
            rtbMiniText.Invoke(new Action(() => rtbMiniText.Enabled = true));
            tbDescription.Invoke(new Action(() => tbDescription.Enabled = true));
            tbKeywords.Invoke(new Action(() => tbKeywords.Enabled = true));
            tbTitle.Invoke(new Action(() => tbTitle.Enabled = true));
            tbLoginNethouse.Invoke(new Action(() => tbLoginNethouse.Enabled = true));
            tbPassNethouse.Invoke(new Action(() => tbPassNethouse.Enabled = true));
            cbMinitext.Invoke(new Action(() => cbMinitext.Enabled = true));
            cbSEO.Invoke(new Action(() => cbSEO.Enabled = true));
            gpOther.Invoke(new Action(() => gpOther.Enabled = true));
            cbOther.Invoke(new Action(() => cbOther.Enabled = true));
        }

        private string Replace(string text, string nameTovar, string article)
        {
            string discount = Discount();
            string nameText = boldOpen + nameTovar + boldClose;
            text = text.Replace("СКИДКА", discount).Replace("НАЗВАНИЕ", nameText).Replace("АРТИКУЛ", article).Replace("<p><br /></p><p><br /></p><p><br /></p><p>", "<p><br /></p>");
            return text;
        }

        private string ReplaceSEO(string nameSEO, string text, string nameTovar, string oldArticle, string article)
        {
            text = text.Replace("НАЗВАНИЕ", nameTovar).Replace("АРТИКУЛ", oldArticle + ";" + article);

            switch (nameSEO)
            {
                case "title":
                    text = RemoveText(text, 255);
                    break;
                case "description":
                    text = RemoveText(text, 200);
                    break;
                case "keywords":
                    text = RemoveText(text, 100);
                    break;
                default:
                    text = RemoveText(text, 100);
                    break;
            }

            return text;
        }

        private string Discount()
        {
            string discount = "<p style=\"text-align: right;\"><span style=\"font-weight: bold; font-weight: bold;\"> 1. <a href=\"https://bike18.ru/oplata-dostavka\">Выгодные условия доставки по всей России!</a></span></p><p style=\"text-align: right;\"><span style=\"font-weight: bold; font-weight: bold;\"> 2. <a href=\"https://bike18.ru/stock\">Нашли дешевле!? 110% разницы Ваши!</a></span></p><p style=\"text-align: right;\"><span style=\"font-weight: bold; font-weight: bold;\"> 3. <a href=\"https://bike18.ru/service\">Также обращайтесь в наш сервис центр в Ижевске!</a></span></p>";
            return discount;
        }

        private string RemoveText(string text, int v)
        {
            if (text.Length > v)
            {
                text = text.Remove(v);
                if (text.Contains(" "))
                    text = text.Remove(text.LastIndexOf(" "));
                else
                    text = text.Remove(text.LastIndexOf(" "));
            }
            return text;
        }

        private void btnImages_Click(object sender, EventArgs e)
        {
            #region Сохранение паролей
            Properties.Settings.Default.login = tbLoginNethouse.Text;
            Properties.Settings.Default.password = tbPassNethouse.Text;
            Properties.Settings.Default.Save();
            #endregion

            #region Обработка сайта

            Thread tabl = new Thread(() => UploadImages());
            forms = tabl;
            forms.IsBackground = true;
            forms.Start();

            #endregion
        }

        private void UploadImages()
        {
            cookie = nethouse.CookieNethouse(tbLoginNethouse.Text, tbPassNethouse.Text);

            if (cookie.Count == 1)
            {
                MessageBox.Show("Логин или пароль для сайта Nethouse введены не верно", "Ошибка логина/пароля");
                return;
            }

            ControlsFormEnabledFalse();

            countUpdateImage = 0;
            otv = GetRequest("https://bike18.ru/products/category/zapchasti-dlya-pitbikov");
            MatchCollection razdel = new Regex("(?<=</div></a><div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);

            for (int i = 0; razdel.Count > i; i++)
            {
                otv = GetRequest("http://bike18.ru" + razdel[i].ToString() + "?page=all");
                MatchCollection tovar = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*(?=\" >)").Matches(otv);
                foreach (Match s in tovar)
                {
                    string urlTovar = s.ToString();
                    UpdateImage(urlTovar);
                }
            }

            MessageBox.Show("Обновленно " + countUpdateImage + "карточек товара");
            ControlsFormEnabledTrue();
        }

        private void UpdateImage(string urlTovar)
        {
            bool b = false;
            string articl = null;
            string images = null;
            string alsoby = null;
            string productGroupe = null;

            List<string> listProd = nethouse.GetProductList(cookie, urlTovar);
            articl = listProd[6];
            images = listProd[32];
            alsoby = listProd[42];
            productGroupe = listProd[3];

            if (listProd[9] == "")
                listProd[9] = "0";

            if (images == "")
            {
                if (File.Exists("Pic\\" + articl + ".jpg"))
                {
                    nethouse.UploadImage(cookie, urlTovar.ToString());
                    b = true;
                    countUpdateImage++;
                }
            }

            if (alsoby.Contains("&alsoBuy[0]=0") || alsoby.Contains("&alsoBuy[0]=als"))
            {
                listProd[42] = nethouse.alsoBuyTovars(listProd);
                b = true;
                countUpdateImage++;
            }

            if (productGroupe != "10833347")
            {
                listProd[3] = "10833347";
                b = true;
                countUpdateImage++;
            }

            if (b)
                nethouse.SaveTovar(cookie, listProd);
        }

        private void cbOther_CheckedChanged(object sender, EventArgs e)
        {
            if (gpOther.Visible)
                gpOther.Visible = false;
            else
                gpOther.Visible = true;
        }
    }
}
