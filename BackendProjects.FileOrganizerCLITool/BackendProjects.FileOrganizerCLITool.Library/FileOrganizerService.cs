namespace BackendProjects.FileOrganizerCLITool.Library
{
    public static class FileOrganizerService
    {
        static readonly Dictionary<string, FileCategory> ExtensionMap = new()
        {
            [".jpg"] = FileCategory.Pictures,
            [".png"] = FileCategory.Pictures,
            [".svg"] = FileCategory.Pictures,
            [".mp3"] = FileCategory.Music,
            [".txt"] = FileCategory.Documents,
            [".csv"] = FileCategory.Documents,
            [".pdf"] = FileCategory.Documents
        };

        public static void OrganizeFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

            var files = Directory.GetFiles(folderPath);

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file);

                if(ExtensionMap.TryGetValue(ext, out var category))
                {
                    string targetDir = Path.Combine(folderPath, category.ToFolderName());
                    Directory.CreateDirectory(targetDir);

                    string fileName = Path.GetFileName(file);
                    string destination = Path.Combine(targetDir, fileName);

                    try
                    {
                        File.Move(file, destination);
                        Console.WriteLine($"Moved: {fileName} → {category.ToFolderName()}/");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Error moving file {fileName}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Skipped (unknown type): {Path.GetFileName(file)}");
                }
            }
        }

        public static string ToFolderName(this FileCategory category)
        {
            return category switch
            {
                FileCategory.Pictures => "Pictures",
                FileCategory.Music => "Music",
                FileCategory.Documents => "Documents",
                _ => "Unknown"
            };
        }
    }
}
