using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Database.Editor
{
    public class SpecialDatabaseEditor : EditorWindow
    {
        SpecialsDatabase specialsDatabase;
        Texture2D selectedTexture;
        Special selectedItem;
        int selectedIndex = -1;
        Vector2 scrollPos;
        State current = State.List;
        
        //constants
        const int SPRITE_BUTTON_SIZE = 46;
        const string FILE_NAME = @"SpecialsDatabase.asset";
        const string FOLDER_PATH = @"Database";
        const string FULL_PATH = @"Assets/" + FOLDER_PATH + "/" + FILE_NAME;

        enum State
        {
            List,
            Edit
        }


        [MenuItem("Custom/Database/Specials Editor %#&S")]
        public static void Init()
        {
            SpecialDatabaseEditor window = EditorWindow.GetWindow<SpecialDatabaseEditor>();
            window.minSize = new Vector2(400, 300);
            window.titleContent.text = "Specials Database";
            window.Show();
        }

        void OnEnable()
        {
            specialsDatabase = ScriptableObject.CreateInstance<SpecialsDatabase>();
            specialsDatabase = specialsDatabase.GetDatabase<SpecialsDatabase>(FOLDER_PATH, FILE_NAME);
        }

        void OnGUI()
        {
            if (specialsDatabase == null)
            { return; }

            if (current == State.List)
            {
                ListView();
                GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
                BottomBar();
                GUILayout.EndHorizontal();
            }
            else if (current == State.Edit)
            {
                if (selectedIndex >= 0)
                {
                    Edit(selectedIndex);
                }
                else Edit(0);
            }

            if (selectedIndex < 0 || selectedIndex > specialsDatabase.Count)
            {
                current = State.List;
            }




            // TODO stop calling this every damn GUI frame but find the right places to call it.
            Repaint();
        }

        void BottomBar()
        {
            GUILayout.Label("Total Specials: " + specialsDatabase.Count);   
            if (GUILayout.Button("Add"))
            {
                specialsDatabase.Add(new Special());
            }
        }

        void ListView()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));

            DisplaySpecials();

            EditorGUILayout.EndScrollView();

        }

        void DisplaySpecials()
        {
            for (int i = 0; i < specialsDatabase.Count; i++)
            {
                GUILayout.BeginHorizontal("Box");

                // Sprite
                if (specialsDatabase.Get(i).Icon)
                {
                    selectedTexture = specialsDatabase.Get(i).Icon.texture;
                }
                else  {selectedTexture = null;}

                if (GUILayout.Button(selectedTexture, GUILayout.Width(SPRITE_BUTTON_SIZE), GUILayout.Height(SPRITE_BUTTON_SIZE)))
                {
                    current = State.Edit;
                    selectedIndex = i;
                }

                //name
                 GUILayout.Label(specialsDatabase.Get(i).Name, GUILayout.Width(100));


                // 3Char
                // Description
                GUILayout.Label(specialsDatabase.Get(i).Description, GUILayout.Height(SPRITE_BUTTON_SIZE));
                // enhance enhancedAttribute

                GUILayout.EndHorizontal();
                
            }
        }


        void Edit(int selected)
        {
            selectedItem = specialsDatabase.Get(selected);
            EditorStyles.textField.wordWrap = true;


            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            // Fields to show/change the name and 3 character designation of the selected special
            selectedItem.Name = EditorGUILayout.TextField("Name:", selectedItem.Name, GUILayout.Width(330));
            selectedItem.CharDesig = EditorGUILayout.TextField("3 Char Desig:", selectedItem.CharDesig);

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();

            // Shows and lets you change the sprite
            // Finds the sprites texture if there is one
            if (specialsDatabase.Get(selected).Icon)
            {
                selectedTexture = specialsDatabase.Get(selected).Icon.texture;
            }
            else { selectedTexture = null; }

            if (GUILayout.Button(selectedTexture, GUILayout.Width(SPRITE_BUTTON_SIZE), GUILayout.Height(SPRITE_BUTTON_SIZE)))
            {
                int controlerID = EditorGUIUtility.GetControlID(FocusType.Passive);
                EditorGUIUtility.ShowObjectPicker<Sprite>(null, true, null, controlerID);
            }

            string commandName = Event.current.commandName;
            if (commandName == "ObjectSelectorUpdated")
            {
                selectedItem.Icon = (Sprite)EditorGUIUtility.GetObjectPickerObject();
                Repaint();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            selectedItem.Description = EditorGUILayout.TextField("Description:", selectedItem.Description, GUILayout.ExpandWidth(true), GUILayout.Width(330), GUILayout.Height(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
            {
                if (selectedItem == null || selectedItem.Name == "") { return; }

                specialsDatabase.Replace(selected, selectedItem);
                current = State.List;

            }
            
            if (GUILayout.Button("Cancel"))
            {
                current = State.List;
               
            }

            if (GUILayout.Button("Delete"))
            {
                if (EditorUtility.DisplayDialog("Delete Special",
                    "Are you sure that you want to delete " + selectedItem.Name + "  special ability forever???",
                    "Delete",
                    "Cancel"))
                {
                    specialsDatabase.Remove(selected);
                    current = State.List;
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}