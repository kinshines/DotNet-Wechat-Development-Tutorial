using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model;
using wxBase.Model.Media;

namespace WebApplicationWeixin.Controllers
{
    public class MediaController : Controller
    {
        public object WeixinService { get; private set; }

        // GET: Media
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(FormCollection form)
        {

            //接收上传的文件
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                return View();
            }
            //上传的文件
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return View();
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase uploadfile = Request.Files[0];
                int pos = Request.Files[0].FileName.LastIndexOf('.');
                string ext = Request.Files[0].FileName.Substring(pos, Request.Files[0].FileName.Length - pos);
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string path = Server.MapPath("/upload");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "//" + newFile;
                uploadfile.SaveAs(path);

                #region 讲图片上传至微信服务器                    
                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", weixinService.Access_token, "image");
                string json = wxMediaService.HttpUploadFile(url, path);
                #endregion
                UpoladThumbResult um = JSONHelper.JSONToObject<UpoladThumbResult>(json);
                Response.Write("上传成功。媒体id:" + um.thumb_media_id + "");
                return View();
                //JObject jb = (JObject)JsonConvert.DeserializeObject(json);//这里就能知道返回正确的消息了下面是个人的逻辑我就没写不用看省略的
            }
        }

        [HttpPost]
        public ActionResult Upload_thumb(FormCollection form)
        {

            //接收上传的文件
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                return View();
            }
            //上传的文件
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return View();
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase uploadfile = Request.Files[0];
                int pos = Request.Files[0].FileName.LastIndexOf('.');
                string ext = Request.Files[0].FileName.Substring(pos, Request.Files[0].FileName.Length - pos);
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string path = Server.MapPath("/upload");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "//" + newFile;
                uploadfile.SaveAs(path);

                #region 讲图片上传至微信服务器                    
                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", weixinService.Access_token, "thumb");
                string json = wxMediaService.HttpUploadFile(url, path);
                #endregion
                UpoladThumbResult um = JSONHelper.JSONToObject<UpoladThumbResult>(json);
                Response.Write("上传成功。媒体id:" + um.thumb_media_id + "");
                return View();
                //JObject jb = (JObject)JsonConvert.DeserializeObject(json);//这里就能知道返回正确的消息了下面是个人的逻辑我就没写不用看省略的
            }
        }

        [HttpPost]
        public ActionResult add_material(FormCollection form)
        {

            //接收上传的文件
            if (Request.Files.Count == 0)
            {
                
                //Request.Files.Count 文件数为0上传不成功
                return View();
            }
            //上传的文件
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return View();
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase uploadfile = Request.Files[0];
                int pos = Request.Files[0].FileName.LastIndexOf('.');
                string ext = Request.Files[0].FileName.Substring(pos, Request.Files[0].FileName.Length - pos);
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string path = Server.MapPath("/upload");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "//" + newFile;
                uploadfile.SaveAs(path);

                #region 将图片上传至微信服务器                    
                string url = string.Format("http://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}", weixinService.Access_token);
                string json = add_material(url, path);
                #endregion
                UpoladThumbResult um = JSONHelper.JSONToObject<UpoladThumbResult>(json);
                Response.Write("上传成功。媒体id:" + um.thumb_media_id + "");
                return View();
            }
        }

        public ActionResult add_news()
        {
            string content = "{\"articles\": [{" +
      "\"title\":\"中国民营企业500强发布，华为超联想夺第一\"," +
      "\"thumb_media_id\":\"o0clbQunWmKo0ud5NpiRgk5dWq9hDQU-eds_YmoD-YA-yG_BN5j0xteBM1R5qsUv\"," +
      "\"author\": \"亿欧\"," +
      "\"digest\": \"今天上午，2016中国民营企业500强发布会在北京召开。榜单显示，华为控股有限公司以营收总额3590.09亿排名第一，苏宁控股、山东魏桥集团分别以3502.88亿、3332.38亿分列二三位。\",\"show_cover_pic\":\"1\"," +
      "\"content\": \"亿欧8月25日消息：今天上午，2016中国民营企业500强发布会在北京召开。榜单显示，华为控股有限公司以营收总额3590.09亿排名第一，苏宁控股、山东魏桥集团分别以3502.88亿、3332.38亿分列二三位。联想、正威国际、大连万达、中国华信、恒力集团、江苏沙钢、万科，分列四至十位。此外，全国工商联经济部部长谭林发布了《2016中国民营企业500强发布报告》。报告显示，2015年，民营企业500强入围门槛达101.75亿元，比上年的95.09亿元净增6.66亿元。2015年，民营企业500强资产总额为173004.87亿元，户均346.01亿元，增幅达25.16 %。从行业角度看，民营企业500强前10大行业，呈现出由传统产业向新兴产业调整的趋势，其中，零售业入围的企业数量同比出现了减少。下面，亿欧将500强中19家零售业企业整理如下：苏宁控股集团 （第2名）苏宁创立于1990年，员工18万人，在中国和日本拥有两家上市公司，下设苏宁云商，苏宁置业，苏宁金控，苏宁文创，苏宁体育和苏宁投资六大产业集团，形成商业、地产、金融、文创、体育、投资六大产业协同发展的格局。其中，苏宁云商面对互联网、物联网、大数据时代，坚持零售本质，持续推进O2O变革，全品类经营，全渠道运营，全球化拓展等，目前，苏宁连锁网络覆盖海内外600多个城市，拥有近1600家门店，其中，线上平台苏宁易购处于中国B2C市场前三。京东集团（第11名）京东于2004年正式涉足电商领域，2014年5月，京东集团在美国纳斯达克证券交易所正式挂牌上市；是中国第一个成功赴美上市的大型综合型电商平台；2015年7月，京东入选纳斯达克100指数和纳斯达克100平均加权指数。截至目前，京东集团拥有近11万名正式员工，业务涉及电商、金融和技术三大领域。目前，京东商城、京东到家、跨境电商、京东金融、京东技术是京东集团的五大部分。三胞集团有限公司（第19名）三胞集团有限公司，是一家以信息化为特征、以现代服务业为基础的大型民营企业集团，以大数据为核心，构建“金融、健康、消费”三大产业，形成“金、木、水、火、土”五大行业协同发展的产业生态圈，努力成为有中国特色、可持续发展的世界级企业组织。集团现拥有宏图高科、南京新百、万威国际、金鹏源康、富通电科等多家上市公司，以及宏图三胞、乐语通讯、宏图地产、广州金鹏、中国新闻周刊、麦考林、拉手网、商圈网、英国House ofFraser、美国Brookstone、以色列Natali等国内外重点企业，下属独资及控股企业超过100家，全球员工总数超过9万人，其中海外员工3万人。新疆广汇实业投资（集团）有限责任公司（第20名）新疆广汇实业投资（集团）有限责任公司创建于1989年，经过27年发展，形成了“能源开发、汽车服务、房产置业”三大产业。2015年，集团完成经营收入1053亿元，实现净利润34亿元，是西北地区唯一一家总资产、经营收入均突破千亿大关的民营企业，员工总数超过8万名。在全面布局三大产业的同时，广汇集团还打造广汇男篮、广汇雪莲堂美术馆两大知名品牌。广汇男篮1999年成立，2002年进入中国男子篮球职业联赛（CBA）。物美控股集团有限公司（第121名）物美自1994年12月在北京率先创办综合超市以来，已经拥有大卖场、生活超市、便利商店、百货店、家居商场等各种业态。物美在中国建立了服务城乡居民的连锁零售网络，物美店铺覆盖京津冀、江浙沪、陕甘宁、以及粤、鲁、闽、鄂、云、贵、川、渝等地，年销售额超过400亿元，年纳税额超18亿元，位列中国连锁百强前茅。此外在2014年，物美收购控股了英资在华企业中国百安居。步步高集团（第138名）1995年3月，步步高集团由王填创立于湖南湘潭，目前有商业、置业、金融三大版块，涉及零售业、电子商务、商业地产、互联网金融、大型物流等多业态的大型商业集团。线下业务主要集中于湖南、江西、广西、四川、重庆、云南、贵州等西南区域，20年来，公司逐步发展成为拥有超市、家电、百货、购物中心、便利店、物流、电子商务、服装、餐饮等多业态的大型商业集团，拥有门店452家；线上业务已经在全国乃至全球铺开。弘阳集团有限公司（第140名）弘阳集团1996年创立于江苏南京，前身为香港红太阳集团，现已形成以商业运营、地产开发、物业服务三大产业协同发展，具有投融资能力的综合性国际企业集团。目前，弘扬产业有以下6大板块：城市综合体（弘阳广场）、地产项目、家居建材、物流运输、星级酒店和物业服务。北京运通国融投资有限公司（第206名）运通集团创立于20世纪80年代，成立于黑龙江哈尔滨市，20余年来运通致力于汽车行业的发展，已从最初的一家维修工厂发展到现今拥有几十家专业4S店的集团公司，成为中国最有影响力的汽车服务企业之一。 今后集团化、跨地域经营、品牌化管理是运通集团的发展模式；多品牌集团化的经营模式，降低了企业经营风险；从高端产品到中级产品，合理的产品布局为运通赢得更大的市场份额。在立足于汽车服务行业的同时，运通集团的业务正朝着多元化方向发展，范围已拓展到了房地产等投资领域，并凭借着良好的业绩，为建立新的品牌4S店提供雄厚的资金支持和充足的土地资源，让我们能够实现短期内完成规划申请和建设工作。唯品会（中国）有限公司（第213名）唯品会成立于2008年08月，总部设在广州，是一家专门做特卖的网站。主营业务为互联网在线销售品牌折扣商品，销售产品涵盖中高端服装、鞋子、箱包、家居用品、化妆品、奢侈品等。2012年3月23日，公司在美国纽交所上市，是华南首家在美国纽交所上市的电子商务企业。唯品会的商业模式是“名牌折扣+限时抢购+正品保险”的创新商业模式，每天早上10点准时上12-18个新品牌，超低折扣，限时抢购。江苏文峰集团有限公司（第221名）江苏文峰集团是以商贸业、酒店业为发展主体的综合型企业集团，集团旗下拥有五星级的南通有斐大酒店、四星级的南通大饭店和文峰饭店，三星级标准的上海家宜宾馆以及多家商务连锁酒店。集团控股的连锁商业企业—文峰大世界连锁发展股份有限公司，拥有多种形态的连锁企业800多家。2011年6月“文峰股份”在上证所成功上市，成为市值百亿的上市公司。上海均瑶（集团）有限公司 （第251名）均瑶集团是以实业投资为主的现代服务业企业，创始于1991年7 月。现以航空运输、商业零售和金融服务为三大主营业务，涉及教育服务、信息科技、投资等领域，现有员工一万多人。 商业零售板块中的核心企业大东方是江苏省百货零售的龙头企业，也是无锡市首家上市的商贸流通企业集团。均瑶如意文化是国内顶级的品牌特许经营商，是北京2008年奥运会特许经营商和零售商，中国2010年上海世博会首批高级赞助商，现与上海迪士尼等开展合作。万马联合控股集团有限公司（第258名）万马联合控股集团有限公司是中国民营企业集团，业务覆盖电气电缆、医疗器械制造、通信电子、房 地产开发、新能源、有色金属贸易等多个领域。面对新的市场竞争格局，万马联合控股集团将秉承“正人、正事、正品”的企业文化，坚持“素质决定实力”的理念，大力 推进技术创新、资本运营、国际化拓展等战略，全面推动万马集团向现代化、国际化企业进一步转型，全力打造“创新型企业”、“实力型企业”，实现“百年万马”的目标。金花投资控股集团有限公司（第269名）金花投资集团成立于1991年，现已发展成为涉足投资、制药、商贸、高科技、电子商务、酒店及高尔夫、教育等领域与产业，拥有两家上市公司，员工两万名，总资产近400亿元人民币的大型企业集团。山东远通汽车贸易集团有限公司（第305名）远通集团成立于1976年，现有资产50亿元，员工7000人，目前，在山东及周边地区建有19个汽车经营园区，20多家汽车零部件代理中心库，经营36个个汽车品牌、72个配件品牌、100多个汽车用品品牌，建有100家县域营销网点，形成独具特色的市县乡村四级营销服务网络体系，总营销服务面积达200多万平方米。润东汽车集团有限公司（第329名）润东汽车是一家专注在豪华高端汽车品牌为主导的综合服务商。成立于1998年，于2001年9月在江苏徐州开设首家汽车经销店后经过快速发展已成为总部位于华东地区规模最大的豪华/超豪华汽车经销商之一。集团旗下网络店面数量截止到2015年底共计70家，主要服务于江苏、山东、上海、浙江等以华东区域为主的沿海富庶地区城市消费者。于2014年8月12日在香港联交所主板成功上市。浙江东杭控股集团有限公司 （第407名）浙江东杭控股集团有限公司是胡宝泉驾驭的、全自然人出资的集团型民营企业。集团现有业务涵盖钢材贸易、工业制造和铁矿资源开采、房产经营等三大朝阳产业链。集团下设11家子公司，净资产4.5亿元,总资产10亿多元，母公司注册资金为1亿元。江苏华地国际控股集团有限公司 （第446名）江苏华地国际控股集团有限公司是一家以长江三角洲地区为战略重心，专注于零售连锁领域的投资和管理的大型企业集团，并于2010年在香港主板成功上市。经过数十年的发展，华地国际已发展成为泛长江三角洲同时经营百货店及超市业务的领先双模式零售连锁店经营商，形成“八佰伴”、“华地百货”、“大统华”三大核心品牌，网点布局跨江苏、安徽两省南京、无锡、镇江、马鞍山等近十个城市。截至2014年底，华地国际拥有近50家大型连锁门店，经营面积120万余平方米。欧龙汽车贸易集团有限公司 （第457名）欧龙汽车集团成立于1995年2 月，由林建忠投资设立的全国无区域综合性汽车集团公司。业务范围涵盖机动车驾驶学校、新车销售、汽车维修、配件销售、二手车业务、汽车俱乐部、汽车衍生服务及机动车检测等完整汽车产业链的专业化汽车及汽车服务企业。经过21年经营，欧龙集团现有奔驰、捷豹、路虎、克莱斯勒、道奇、JEEP、广汽菲克、一汽-大众、福特、雪佛兰、东风标致、东风雪铁龙等15个知名汽车品牌30余家品牌经营店，1家机动车检测服务中心及1家汽车驾培学校，产品覆盖面广，能够满足不同客户群体的需求，为浙南地区经营规模最大的汽车经销商集团之一。成都红旗连锁股份有限公司 （第500名）红旗连锁创建于2000年6月22日。2010年6月9日，整体变更为成都红旗连锁股份有限公司。公司已发展成为中国西部地区最具规模的以连锁经营、物流配送、电子商务为一体的商业连锁企业，是中国A股市场首家便利连锁超市上市企业。目前在四川省内已开设2400余家连锁超市，就业员工17000人，2015年上缴税收和社保超4亿元；拥有四座物流配送中心。本文作者郭之富，亿欧专栏作者；微信：guo_05（添加时请注明“姓名-公司-职务”方便备注）；转载请注明作者姓名和“来源：亿欧”；文章内容系作者个人观点，不代表亿欧对观点赞同或支持。\","
      + "\"content_source_url\":\"http://iyiou.baijia.baidu.com/article/600491\" }" + "] }";
            UploadMediaResult result = JSONHelper.JSONToObject<UploadMediaResult>(wxMediaService.add_news(content));
            Response.Write("mediaid:"+ result.media_id);
            return View();
        }

        [HttpPost]
        public ActionResult Get(FormCollection form)
        {
            // 创建images目录
            string images_dir = Server.MapPath("~/images/");
            if (!Directory.Exists(images_dir))
                Directory.CreateDirectory(images_dir);
            string mediaid = form["Mediaid"];
            string filename = DateTime.Now.ToString("yyyyMMddHHmmddfff") + ".jpg";
            string path = images_dir + "\\" + filename;
            wxMediaService.Get(mediaid, path);
            Response.Redirect("~/images/" + filename);
            return View();
        }


        public static string add_material(string url, string path)
        {
            var boundary = "fbce142e-4e8e-4bf3-826d-cc3cf506cccc";
            var client = new HttpClient();  //使用HttpClient对象将永久图片上传至微信服务器
            client.DefaultRequestHeaders.Add("User-Agent", "KnowledgeCenter");
            //  准备默认请求包头
            client.DefaultRequestHeaders.Remove("Expect");
            client.DefaultRequestHeaders.Remove("Connection");
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.ConnectionClose = true;
            //将图片内容放入请求包体
            var content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
            //处理图片
            Image image = Image.FromFile(path);
            byte[] ImageByte = ImageToBytes(image);
            var contentByte = new ByteArrayContent(ImageByte);
            content.Add(contentByte);
            string filename = Path.GetFileName(path);
            string ext = Path.GetExtension(path).ToLower();
            string content_type = "";
            if (ext == ".jpg" || ext == ".jpeg")
                content_type = "image/jpeg";
            if (ext == ".png")
                content_type = "image/png";
            if (ext == ".gif")
                content_type = "image/gif";

            contentByte.Headers.Remove("Content-Disposition");
            contentByte.Headers.TryAddWithoutValidation("Content-Disposition", "form-data; name=\"media\";filename=\"" + filename + "\"" + "");
            contentByte.Headers.Remove("Content-Type");
            contentByte.Headers.TryAddWithoutValidation("Content-Type", content_type);
            try
            {
                //提交至微信服务器
                var result = client.PostAsync(url, content);
                if (result.Result.StatusCode != HttpStatusCode.OK)
                    throw new Exception(result.Result.Content.ReadAsStringAsync().Result);
                // 返回结果
                if (result.Result.Content.ReadAsStringAsync().Result.Contains("media_id"))
                {
                    var resultContent = result.Result.Content.ReadAsStringAsync().Result;
                    //    var materialEntity = JsonConvert.DeserializeObject<MaterialImageReturn>(resultContent);

                    return resultContent;
                }
                throw new Exception(result.Result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException.Message);
            }

        }

        public static byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }

        }

        [HttpPost]
        public ActionResult Upload_newsimg(FormCollection form)
        {

            //接收上传的文件
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                return View();
            }
            //上传的文件
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return View();
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase uploadfile = Request.Files[0];
                int pos = Request.Files[0].FileName.LastIndexOf('.');
                string ext = Request.Files[0].FileName.Substring(pos, Request.Files[0].FileName.Length - pos);
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string path = Server.MapPath("/upload");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "//" + newFile;
                uploadfile.SaveAs(path);

                #region 讲图片上传至微信服务器                    
                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/uploadimg?access_token="+ weixinService.Access_token);
                string json = wxMediaService.HttpUploadFile(url, path);
                #endregion
                UpoladThumbResult um = JSONHelper.JSONToObject<UpoladThumbResult>(json);
                Response.Write("上传成功。媒体id:" + um.thumb_media_id + "");
                return View();
                //JObject jb = (JObject)JsonConvert.DeserializeObject(json);//这里就能知道返回正确的消息了下面是个人的逻辑我就没写不用看省略的
            }
        }

        [HttpPost]
        public ActionResult Upload_voice(FormCollection form)
        {

            //接收上传的文件
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                return View();
            }
            //上传的文件
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return View();
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase uploadfile = Request.Files[0];
                int pos = Request.Files[0].FileName.LastIndexOf('.');
                string ext = Request.Files[0].FileName.Substring(pos, Request.Files[0].FileName.Length - pos);
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string path = Server.MapPath("/upload");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "//" + newFile;
                uploadfile.SaveAs(path);

                #region 讲图片上传至微信服务器                    
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=" + weixinService.Access_token);
                string json = wxMediaService.HttpUploadFile(url, path);
                #endregion
                UpoladThumbResult um = JSONHelper.JSONToObject<UpoladThumbResult>(json);
                Response.Write("上传成功。媒体id:" + um.thumb_media_id + "");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Get_Material(FormCollection form)
        {
            // 创建images目录
            string images_dir = Server.MapPath("~/images/");
            if (!Directory.Exists(images_dir))
                Directory.CreateDirectory(images_dir);
            string mediaid = form["Mediaid"];
            string filename = DateTime.Now.ToString("yyyyMMddHHmmddfff") + ".jpg";
            string path = images_dir + filename;
            wxMediaService.Get_material(mediaid, path);
            Response.Redirect("~/images/" + filename);
            return View();
        }
        
        [HttpPost]
        public ActionResult update_News(FormCollection form)
        {
            string mediaid = form["Mediaid"];
            string content = "{\"media_id\":\""+mediaid+"\", \"index\": 0,\"articles\": {" +
      "\"title\":\"1111\"," +
      "\"thumb_media_id\":\"zqjVV1lKFDcpg5kFgCh6ZKTn5um8_RZ_Y3B1EF_CUW9xAbOc41Sagi4z4pEnTn2y\"," +
      "\"author\": \"22\"," +
      "\"digest\": \"33。\",\"show_cover_pic\":1," +
      "\"content\": \"44\","
      + "\"content_source_url\":\"55\" } }";

            string url = "https://api.weixin.qq.com/cgi-bin/material/update_news?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, content);
            return View();
        }

        [HttpPost]
        public ActionResult delete(string Mediaid)
        {
            wxResult result = JSONHelper.JSONToObject<wxResult>(wxMediaService.delete(Mediaid));
            if (result.errcode == "0")
                Response.Write("操作成功");
            else
                Response.Write("操作失败：" + result.errmsg);

            return View();
        }

        public ActionResult get_materialcount()
        {
            string json = wxMediaService.get_materialcount();
            wxMaterialcount mcount = JSONHelper.JSONToObject<wxMaterialcount>(json);
            Response.Write("您的公众号共有"+ mcount.voice_count+"个语音消息，"+mcount.image_count+"个图片消息，"+mcount.news_count+"个图文消息，"+mcount.video_count+"个视频消息。");
            return View();
        }

        public ActionResult batchget_material()
        {
            string json = wxMediaService.batchget_material("image");
            wxMateriallist mlist = JSONHelper.JSONToObject<wxMateriallist>(json);
            Response.Write("您的公众号共有" + mlist.total_count + "个图片素材，本次获取"+mlist.item_count+"个素材信息。具体如下：<br/><br/>" );
            for (int i = 0; i < mlist.item.Count; i++)
            {
                Response.Write("素材id:" + mlist.item[i].media_id + "<br/>");
                Response.Write("素材名字:" + mlist.item[i].name + "<br/>");
                Response.Write("最后更新时间:" + mlist.item[i].update_time + "<br/>");
                Response.Write("url:" + mlist.item[i].url + "<br/>");
            }
            return View();
        }

    }
}