using System.Text;
using Newtonsoft.Json;

namespace MalodyKey2DR3;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("错误：请输入要转的谱面（路径）");
        }
        else
        {
            MalodyChart? chart = JsonConvert.DeserializeObject<MalodyChart>(File.ReadAllText(args[0]));
            if (chart == null) throw new ArgumentException("谱面格式非法");
            if (chart.meta.mode != 0) throw new ArgumentException("你tm把key以外的谱面传进来做啥");
            if (chart.meta.modeExt.columnCount != 4) throw new ArgumentException("暂时不支持4k以外的key模式喵");
            int id = 0;
            List<string> drbLines = new List<string>();
            MalodyNote specialNote = chart.noteList[^1];
            if (string.IsNullOrEmpty(specialNote.sound)) throw new ArgumentException("谱面格式非法");
            drbLines.Add($"#OFFSET={specialNote.offset / -1000f:N2};");
            drbLines.Add("#BEAT=1;");
            drbLines.Add($"#BPM_NUMBER={chart.bpmList.Length};");
            for (var i = 0; i < chart.bpmList.Length; i++)
            {
                var bpm = chart.bpmList[i];
                drbLines.Add($"#BPM [{i}]={bpm.bpm};");
                drbLines.Add(
                    $"#BPMS[{i}]={Util.FloatToDRBDecimal(MalodyBeatToDR3Beat(bpm.time, chart.meta.modeExt.barBegin))};");
            }

            drbLines.Add("#SCN=1;");
            drbLines.Add("#SC [0]=1;");
            drbLines.Add("#SCI[0]=0.000;");
            for (var i = 0; i < chart.noteList.Length - 1; i++)
            {
                var note = chart.noteList[i];
                if (note.time != null)
                {
                    if (note.time.Count == 3)
                    {
                        drbLines.Add(
                            $"<{id}><1><{MalodyBeatToDR3Beat(note.time.ToArray(), chart.meta.modeExt.barBegin):N5}><{note.column * 4}><4><1><0>");

                        id++;
                    }
                }

                if (note.endTime != null)
                {
                    if (note.endTime.Count == 3)
                    {
                        drbLines.Add(
                            $"<{id}><6><{MalodyBeatToDR3Beat(note.endTime.ToArray(), chart.meta.modeExt.barBegin):N5}><{note.column * 4}><4><0><{id - 1}>");
                        id++;
                    }
                }
            }

            using StreamWriter streamWriter =
                new StreamWriter(
                    Path.GetDirectoryName(args[0]) + "/" + Path.GetFileNameWithoutExtension(args[0]) + ".txt",
                    false, new UTF8Encoding(false));
            foreach (string drbLine in drbLines)
            {
                streamWriter.WriteLine(drbLine);
            }
        }

        Console.WriteLine("按任意键继续...");
        Console.ReadKey();
    }

    private static float MalodyBeatToDR3Beat(int[] time, int barBegin)
    {
        return MalodyBeatToDR3Beat(time[0], time[1], time[2], barBegin);
    }

    private static float MalodyBeatToDR3Beat(int beat, int pos, int divide, int barBegin)
    {
        if (beat == barBegin) return 0f;
        float fBeat = (beat - barBegin) / 4f;
        if (pos == 0) return fBeat;
        return fBeat + pos / 4f / divide;
    }
}