using UnityEngine;
using System;
using System.Collections.Generic;

namespace MalbersAnimations.Utilities
{
    /// <summary>
    /// This Mono is used to change Materials on any Mesh Renderer using a list of Materials Items
    /// </summary>
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField]
        public List<MaterialItem> materialList = new List<MaterialItem>();
        [HideInInspector]
        [SerializeField]
        public bool showMeshesList = true;


        /// <summary>
        /// Swap to the next or previous material on each Material Item;
        /// </summary>
        public virtual void SetAllMaterials(bool Next = true)
        {
            foreach (var materialItem in materialList)
            {
                materialItem.ChangeMaterial(Next);
            }
        }

        /// <summary>
        /// Set all the MaterialItems on the List a specific Material using an Index
        /// </summary>
        /// <param name="index">the index on the Materials[], for each Material Item</param>
        public virtual void SetAllMaterials(int index)
        {
            foreach (var mat in materialList)
            {
                mat.ChangeMaterial(index);
            }
        }

        /// <summary>
        /// Set a Material from the List of material inside the materialList... wierd, but ir works
        /// </summary>
        /// <param name="indexList">index of the Material List</param>
        /// <param name="indexCurrent">index a material on the MaterialList</param>
        public virtual void SetMaterial(int indexList, int indexCurrent)
        {
            if (indexList < 0) indexList = 0;
            indexList = indexList % materialList.Count;

            if (materialList[indexList] != null)
                materialList[indexList].ChangeMaterial(indexCurrent);
        }


        /// <summary>
        /// Set all the MaterialItems on the List an External Material
        /// </summary>
        /// <param name="mat"></param>
        public virtual void SetAllMaterials(Material mat)
        {
            foreach (var MaterialItem in materialList)
            {
                MaterialItem.ChangeMaterial(mat);
            }
        }


        /// <summary>
        /// Swap to the Next material on a specific Material Item on the List using index
        /// </summary>
        /// <param name="index">index on the Material Item on the material list</param>
        public virtual void NextMaterialItem(int index)
        {
            if (index < 0) index = 0;
            index = index % materialList.Count;

            materialList[index].NextMaterial();
        }

        /// <summary>
        /// Swap to the Next material on a specific Material Item on the List using the Name
        /// </summary>
        /// <param name="name">the Name used for the MaterialItem</param>
        public virtual void NextMaterialItem(string name)
        {
            MaterialItem mat = materialList.Find(item => item.Name == name);

            if (mat != null) mat.NextMaterial();
        }

        /// <summary>
        /// Returns the Current Index on the material list using the index of the slot
        /// </summary>
        public virtual int CurrentMaterialIndex(int index)
        {
            return materialList[index].current;
        }

        /// <summary>
        /// Returns the Current index of the material list slot using the index of the slot
        /// </summary>
        public virtual int CurrentMaterialIndex(string name)
        {
            int index = materialList.FindIndex(item => item.Name == name);
            return materialList[index].current;
        }

    }

    [Serializable]
    public class MaterialItem
    {
        [SerializeField]
        [HideInInspector]
        public string Name;                 //The name for the Material to change
        public Renderer mesh;               //The mesh renderer to use for the materials
        public Material[] materials;        //The list of the Materials

        [HideInInspector]
        [SerializeField]
        public int current = 0;
        public bool HasLODs;
        public Renderer[] LODs;

        #region Constructors
        public MaterialItem()
        {
            Name = "NameHere";
            mesh = null;
            materials = new Material[0];
        }

        public MaterialItem(MeshRenderer MR)
        {
            Name = "NameHere";
            mesh = MR;
            materials = new Material[0];
        }

        public MaterialItem(string name, MeshRenderer MR, Material[] mats)
        {
            Name = name;
            mesh = MR;
            materials = mats;
        }

        public MaterialItem(string name, MeshRenderer MR)
        {
            Name = name;
            mesh = MR;
            materials = new Material[0];
        }
        #endregion

        /// <summary>
        /// Changes to the next material on the list..(Same as NextMaterial)
        /// </summary>
        public virtual void ChangeMaterial()
        {
            current++;
            if (current < 0) current = 0;
            current = current % materials.Length;

            if (materials[current] != null)
            {
                mesh.material = materials[current];
                ChangeLOD(current);
            }
            else
            {
                Debug.LogWarning("The Material on the Slot: " + current + " is empty");
            }
        }

        internal void ChangeLOD(int index)
        {
            if (!HasLODs) return;
            foreach (var mesh in LODs)
            {
                if (materials[current] != null)
                    mesh.material = materials[current];
            }
        }

        internal void ChangeLOD(Material mat)
        {
            if (!HasLODs) return;
            foreach (var mesh in LODs)
            {
                mesh.material = mat;
            }
        }

        /// <summary>
        /// Changes to the Next material on the list.(Same as ChangeMaterial)
        /// </summary>
        public virtual void NextMaterial()
        {
            ChangeMaterial();
        }

        /// <summary>
        /// Used for Change a specific material on the list using and Index.
        /// </summary>
        /// <param name="index">Index for the Material Array</param>
        public virtual void ChangeMaterial(int index)
        {
            if (index < 0) index = 0;
            index = index % materials.Length;

            if (materials[index] != null)
            {
                mesh.material = materials[index];
                current = index;
                ChangeLOD(index);
            }
            else
            {
                Debug.LogWarning("The material on the Slot: " + index + "  is empty");
            }
        }

        /// <summary>
        /// Changes to the previous material on the list.
        /// </summary>
        public virtual void PreviousMaterial()
        {
            current--;
            if (current < 0) current = materials.Length - 1;

            if (materials[current] != null)
            {
                mesh.material = materials[current];
                ChangeLOD(current);
            }
            else
            {
                Debug.LogWarning("The Material on the Slot: " + current + " is empty");
            }
        }

        /// <summary>
        /// Changes to a specific External material
        /// </summary>
        public virtual void ChangeMaterial(Material mat)
        {
            mesh.material = mat;
            ChangeLOD(mat);
        }

        /// <summary>
        /// Changes to the Next or Previous material on the list
        /// </summary>
        /// <param name="Next">true: Next, false: Previous</param>
        public virtual void ChangeMaterial(bool Next = true)
        {
            if (Next)
                NextMaterial();
            else
                PreviousMaterial();
        }
    }
}

