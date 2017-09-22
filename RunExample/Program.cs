using System;
using System.IO;
using System.Linq;
using Structurizr;
using Structurizr.Analysis;
using Structurizr.Client;
using Structurizr.Util;

namespace RunExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var workSpaceId = 37192;
            var apiKey = "130be122-6b80-4bfa-8a0b-5106fdf533cd";
            var apiSecret = "7764a340-bf53-4a40-a5c6-37034f7d2648";

            CreateContosoExample(workSpaceId, apiKey, apiSecret);

            Console.WriteLine("Done!");
            Console.ReadKey();

        }

        private static void CreateContosoExample(int workSpaceId, string apiKey, string apiSecret)
        {
            Workspace workspace = new Workspace("Contoso University", "A software architecture model of the Contoso University sample project.");
            Model model = workspace.Model;
            ViewSet views = workspace.Views;
            Styles styles = views.Configuration.Styles;

            Person universityStaff = model.AddPerson("University Staff", "A staff member of the Contoso University.");
            SoftwareSystem contosoUniversity = model.AddSoftwareSystem("Contoso University", "Allows staff to view and update student, course, and instructor information.");

            universityStaff.Uses(contosoUniversity, "uses");

            SystemContextView contextView = views.CreateSystemContextView(contosoUniversity, "Context", "The system context view for the Contoso University system.");
            contextView.AddAllElements();

            #region Containers

            //Container webApplication = contosoUniversity.AddContainer("Web Application", "Allows staff to view and update student, course, and instructor information.", "Microsoft ASP.NET MVC");
            //Container database = contosoUniversity.AddContainer("Database", "Stores information about students, courses and instructors", "Microsoft SQL Server Express LocalDB");
            //database.AddTags("Database");
            //database.Url = "https://github.com/simonbrowndotje/ContosoUniversity/tree/master/ContosoUniversity/Migrations";

            //universityStaff.Uses(webApplication, "Uses", "HTTPS");
            //webApplication.Uses(database, "Reads from and writes to");

            //ContainerView containerView = views.CreateContainerView(contosoUniversity, "Containers", "The containers that make up the Contoso University system.");
            //containerView.AddAllElements();

            #endregion

            #region Dynamic components

            //ComponentFinder componentFinder = new ComponentFinder(
            //    webApplication,
            //    typeof(ContosoUniversity.MvcApplication).Namespace,
            //    new TypeBasedComponentFinderStrategy(
            //        new InterfaceImplementationTypeMatcher(typeof(System.Web.Mvc.IController), null, "ASP.NET MVC Controller"),
            //        new ExtendsClassTypeMatcher(typeof(System.Data.Entity.DbContext), null, "Entity Framework DbContext")
            //    )
            //);
            //componentFinder.FindComponents();

            //// connect the user to the web MVC controllers
            //webApplication.Components.ToList().FindAll(c => c.Technology == "ASP.NET MVC Controller").ForEach(c => universityStaff.Uses(c, "uses"));

            //// connect all DbContext components to the database
            //webApplication.Components.ToList().FindAll(c => c.Technology == "Entity Framework DbContext").ForEach(c => c.Uses(database, "Reads from and writes to"));

            //ComponentView componentView = views.CreateComponentView(webApplication, "Components", "The components inside the Contoso University web application.");
            //componentView.AddAllElements();

            #endregion

            #region Documentation

            //Documentation documentation = workspace.Documentation;
            //FileInfo documentationRoot = new FileInfo(@"..\..\ContosoDocs");
            //documentation.Add(contosoUniversity, SectionType.Context, DocumentationFormat.Markdown,
            //    new FileInfo(Path.Combine(documentationRoot.FullName, "context.md")));
            //documentation.Add(contosoUniversity, SectionType.FunctionalOverview, DocumentationFormat.Markdown,
            //    new FileInfo(Path.Combine(documentationRoot.FullName, "functional-overview.md")));
            //documentation.Add(contosoUniversity, SectionType.QualityAttributes, DocumentationFormat.Markdown,
            //    new FileInfo(Path.Combine(documentationRoot.FullName, "quality-attributes.md")));
            //documentation.AddImages(documentationRoot);

            #endregion

            // add some styling
            styles.Add(new ElementStyle(Tags.Person) { Background = "#0d4d4d", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#003333", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#226666", Color = "#ffffff" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#407f7f", Color = "#ffffff" });

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            structurizrClient.PutWorkspace(workSpaceId, workspace);
        }        
    }
}
