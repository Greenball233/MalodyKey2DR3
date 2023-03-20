using Newtonsoft.Json;

namespace MalodyKey2DR3;

public class MalodyChart
{
    [JsonProperty("meta")] public MalodyChartMeta meta;
    [JsonProperty("time")] public MalodyBpm[] bpmList;
    [JsonProperty("note")] public MalodyNote[] noteList;
}

public class MalodyChartMeta
{
    [JsonProperty("$ver")] public int version;
    [JsonProperty("creator")] public string creator;
    [JsonProperty("background")] public string background;
    [JsonProperty("version")] public string hard;
    [JsonProperty("preview")] public int preview;
    [JsonProperty("id")] public int id;
    [JsonProperty("mode")] public int mode;
    [JsonProperty("time")] public int time;
    [JsonProperty("song")] public MalodyChartMetaSong song;
    [JsonProperty("mode_ext")] public MalodyChartMetaModeExt modeExt;
}

public class MalodyChartMetaSong
{
    [JsonProperty("title")] public string title;
    [JsonProperty("artist")] public string artist;
    [JsonProperty("id")] public int id;
}

public class MalodyChartMetaModeExt
{
    [JsonProperty("column")] public int columnCount;
    [JsonProperty("bar_begin")] public int barBegin;
}

public class MalodyBpm
{
    [JsonProperty("beat")] public int[] time;
    [JsonProperty("bpm")] public float bpm;
}

public class MalodyNote
{
    [JsonProperty("beat")] public List<int> time;
    [JsonProperty("endbeat")] public List<int> endTime;
    [JsonProperty("column")] public int column;
    [JsonProperty("sound")] public string sound;
    [JsonProperty("vol")] public int volume;
    [JsonProperty("offset")] public int offset;
    [JsonProperty("type")] public int type;
}