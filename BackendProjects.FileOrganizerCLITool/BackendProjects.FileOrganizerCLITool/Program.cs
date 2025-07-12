using BackendProjects.FileOrganizerCLITool.Library;

namespace BackendProjects.FileOrganizerCLITool
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FileOrganizerService.OrganizeFiles(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}