namespace FolkerKinzel.DataUrls.Tests;

internal static class TestFiles
{
    private const string TEST_FILE_DIRECTORY_NAME = "TestFiles";
    private static readonly string _testFileDirectory;

    static TestFiles()
    {
        ProjectDirectory = Properties.Resources.ProjDir.Trim();
        _testFileDirectory = Path.Combine(ProjectDirectory, TEST_FILE_DIRECTORY_NAME);
    }


    internal static string[] GetAll() => Directory.GetFiles(_testFileDirectory);


    internal static string ProjectDirectory { get; }

    internal static string FolkerPng => Path.Combine(_testFileDirectory, "Folker.png");
    internal static string EmptyTextFile => Path.Combine(_testFileDirectory, "Empty.txt");
    internal static string Utf8 => Path.Combine(_testFileDirectory, "utf-8.txt");
    internal static string Utf16LE => Path.Combine(_testFileDirectory, "utf-16LE_BOM.txt");



}
