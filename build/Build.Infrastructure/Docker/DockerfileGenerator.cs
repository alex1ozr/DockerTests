using System.Reflection;
using System.Text;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Scriban;

namespace DockerTestsSample.Build.Infrastructure.Docker;

internal static class DockerfileGenerator
{
    public static AbsolutePath GenerateDockerfile(Project project)
    {
        var msBuildProject = project.GetMSBuildProject();
        var assemblyName = msBuildProject.GetPropertyValue("AssemblyName");

        AbsolutePath baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = baseDir / $"{assemblyName}-{Guid.NewGuid()}.Dockerfile";
        var projectRelativePath = project.Solution.Path.Parent.GetRelativePathTo(project.Path);
        
        var publishArtifacts = project.Solution
            .AllProjects
            .Select(x => x.GetMSBuildProject())
            .Any(x => string.Equals(x.GetPropertyValue("IsPackable"), "true", StringComparison.OrdinalIgnoreCase));
        
        RenderDockerfileTemplate(filePath, new DockerfileModel(projectRelativePath, assemblyName, publishArtifacts));

        return filePath;
    }

    private static void RenderDockerfileTemplate(string filePath, DockerfileModel model)
    {
        var text = GetDockerfileTemplateContent();
        var template = Template.Parse(text, filePath);

        var result = template.Render(model, member => member.Name);
        
        File.WriteAllText(filePath, result, Encoding.UTF8);
    }

    private static string GetDockerfileTemplateContent()
    {
        const string templatePath = "DockerTestsSample.Build.Infrastructure.Docker.Template.Dockerfile";
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(templatePath)
                              ?? throw new InvalidOperationException($"Dockerfile template ({templatePath}) not found");
        using var reader = new StreamReader(stream);
        var dockerfileContent = reader.ReadToEnd();

        return dockerfileContent;
    }
}