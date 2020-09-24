using System.Collections.Generic;

namespace GDSHelpers.Models
{
    public class BreadCrumbs
    {
        public BreadCrumbs()
        {
        }
        public BreadCrumbs(bool addHome = true, string homeText = "Home", string homeUrl = "/")
        {
            if(addHome)
                Crumbs.Add(new Crumb(homeText, homeUrl));
        }

        public List<Crumb> Crumbs { get; set; } = new List<Crumb>();
    }

    public class Crumb
    {
        public Crumb()
        {
        }
        public Crumb(string text, string url)
        {
            Text = text;
            Url = url;
        }

        public string Url { get; set; }
        public string Text { get; set; }
    }

}
