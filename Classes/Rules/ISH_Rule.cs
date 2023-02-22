﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeGrammar.Classes.Rules
{
    public interface ISH_Rule
    {
        void NewRuleParameters(Random random, SH_SimpleShape simpleShape);

        SH_Rule CopyRule(SH_Rule rule);
    }
}
