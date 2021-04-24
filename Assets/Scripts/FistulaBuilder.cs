using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FistulaBuilder : MonoBehaviour
{
    [SerializeField] FistulaSegment firstSegment;
    [SerializeField] List<FistulaSegment> segments;
    [SerializeField] int segmentsDeep = 20;
    
    Dictionary<BuilderInstruction, List<BuilderInstruction>> linkOptions = new Dictionary<BuilderInstruction, List<BuilderInstruction>>();

    void Start()
    {
        CreateLinkOptions();
        BuildFistula();
    }

    void CreateLinkOptions()
    {
        HashSet<string> sids = new HashSet<string>();

        int n = segments.Count;
        for (int i=0; i<n;i++)
        {
            FistulaSegment seg = segments[i];
            if (sids.Contains(seg.SegmentID))
            {
                Debug.LogError(string.Format("{0} segment id already in use, skipping {1}", seg.SegmentID, seg.name));
                continue;
            }
            List<BuilderInstruction> outs = new List<BuilderInstruction>();
            List<BuilderInstruction> ins = new List<BuilderInstruction>();
            for (int j=0; j<n; j++)
            {
                FistulaSegment other = segments[j];
                if (seg.Down == other.Up)
                {
                    outs.Add(new BuilderInstruction(other));
                }
                if (seg.Down == other.Down)
                {
                    outs.Add(new BuilderInstruction(other, true));
                }
                if (seg.Up == other.Down)
                {
                    ins.Add(new BuilderInstruction(other, true));
                }
                if (seg.Up == other.Up)
                {
                    ins.Add(new BuilderInstruction(other));
                }
            }
            linkOptions.Add(new BuilderInstruction(seg), outs);
            linkOptions.Add(new BuilderInstruction(seg, true), ins);
            /*
            Debug.Log(string.Format(
                "{0} OUT {1}",
                seg.name,
                string.Join(", ", outs.Select(o => o.toString()))
                )
            );
            Debug.Log(string.Format(
                "{0} IN {1}",
                seg.name,
                string.Join(", ", ins.Select(o => o.toString()))
                )
            );
            */
        }
    }

    void BuildFistula() {
        var seg = SpawnSegment(firstSegment, transform.position, false);
        var instruction = new BuilderInstruction(seg);
        for (int i=1;i<segmentsDeep;i++)
        {
            instruction = getRndInstruction(instruction);
            seg = SpawnSegment(instruction.segment, seg.Bottom, instruction.flipped);
            seg.SpawnDirt(1);            
        }
    }

    BuilderInstruction getRndInstruction(BuilderInstruction previous)
    {
        foreach (var key in linkOptions.Keys)
        {
            if (key.equals(previous)) {
                var options = linkOptions[key];
                return options[Random.Range(0, options.Count)];
            }
        }
        throw new System.Exception("Could not find instruction");
    }

    FistulaSegment SpawnSegment(FistulaSegment prefab, Vector3 refPosition, bool flipped)
    {
        var seg = Instantiate<FistulaSegment>(prefab, refPosition, Quaternion.identity, transform);
        seg.transform.position = seg.Bottom;
        if (flipped)
        {
            seg.transform.localScale = new Vector3(1, -1, 1);
        }
        return seg;
    }

    void Update()
    {
        
    }
}
