using System.IO;
using CommonMark;

namespace MarkGen.Generation {
    public class MarkDownHtmlGenerator {
        public void Generate(string sourceFilePath, string targetFilePath) {
            using (var reader = new StreamReader(sourceFilePath))
            using (var writer = new StreamWriter(targetFilePath)) {
                CommonMarkConverter.Convert(reader, writer);
            }
        }
    }
}