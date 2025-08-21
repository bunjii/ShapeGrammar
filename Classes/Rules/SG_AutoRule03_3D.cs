using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using feb;
using Rhino;
using Rhino.Geometry;

using ShapeGrammar.Classes.Elements;

namespace ShapeGrammar.Classes.Rules
{
    [Serializable]
    public class SG_AutoRule03_3D : SG_Rule
    {

        // --- properties ---

        public List<string> ElemNames { get; set; }
        // public double Rotation_angle { get; set; }
        public double[] Domain { get; set; }


        // --- constructors ---

        public SG_AutoRule03_3D()
        {

        }


        public SG_AutoRule03_3D(List<string> _eNames, double[] _domain)
        {
            RuleState = State.alpha;
            Name = "SH_AutoRule_03_3D";
            ElemNames = _eNames;
            Domain = _domain;
            // Rotation_angle = _angle_rad;
        }

        // --- methods ---
        public override void NewRuleParameters(Random random, SG_Shape ss) { }
        public override SG_Rule CopyRule(SG_Rule rule)
        {
            throw new NotImplementedException();
        }
        public override string RuleOperation(ref SG_Shape ss_ref, ref SG_Genotype gt)
        {

            // algorithm for rule 03-3d

            // find relevant range in genotype
            int sid = -999;
            int eid = -999;
            List<int> selectedIntGenes;
            List<double> selectedDGenes;

            gt.FindRange(ref sid, ref eid, UT.RULE030_MARKER);

            if (sid == -999 || eid == -999)
            {
                return "Autorule03 - wrong marker";
            }

            // extract relevant genes
            selectedIntGenes = gt.IntGenes.GetRange(sid, eid - sid);
            selectedDGenes = gt.DGenes.GetRange(sid, eid - sid);

            double range = Domain[1] - Domain[0];

            var relevantElems = ss_ref.Elems.Where(e => e.Name == "3DAR2").ToList();

            for (int i = 0; i < selectedIntGenes.Count; i++)
            {
                if (selectedIntGenes[i] == 0) continue;
                if (i >= relevantElems.Count) break;

                SG_Elem1D elem = relevantElems[i] as SG_Elem1D;

                double rotationangle = selectedDGenes[i] * range + Domain[0];

                // var e0 = (SG_Elem1D) ss_ref.Elems[0];
                
                var epln = elem.EPln;
                var rotationaxis = elem.EPln.YAxis; 

                //epln.Rotate(Rotation_angle, vx);
                epln.Rotate(rotationangle, rotationaxis);
                elem.EPln = epln;

                elem.Ln = new Line(epln.Origin, epln.ZAxis, elem.Ln.Length);

                elem.Nodes[1].Pt = elem.Ln.To;

                // elem.Name = "3DAR3";

            }

            return "Auto-rule 03-3D successfully applied.";

        }


        public override State GetNextState()
        {
            throw new NotImplementedException();
        }
    }
}
