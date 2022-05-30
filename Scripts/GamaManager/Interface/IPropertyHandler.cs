using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPropertyHandler
{
    int ChapterNum { get; }
    int StageNum { get; }
    int Acorn { get; set; }
}
