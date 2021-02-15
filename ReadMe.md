# Language Support

This mod enables the use of additional languages in a similar way CustomKnight does.

## Adding additional translations

1. Install the mod
2. Download a language pack
3. Run the game once
    1. This generates english translation files
4. Exit the game
5. Go to the `LanguageSupport` folder
    1. This is located next to the `CustomKnight` folder and also next to mod files
6. Extract the language pack here so that the extracted folder has the name of the Language Code of the language it was made for (e.g. `CS` for czech)
7. Start the game
8. The language should now be selectable in the options

## Making additional translations

1. Install the mod
2. Run the game once
    1. This generates english translation files
3. Exit the game
4. Go to the `LanguageSupport` folder
    1. This is located next to the `CustomKnight` folder and also next to mod files
5. Copy the `EN` folder and name it to the Language Code of the language you want to translate to (e.g. `CS` for czech)
6. Translate the files
7. Open `MainMenu.txt`
8. Add a line at the top that says `<entry name="LANG_##">Language Name Here</entry>`, which `##` being the same Language Code from step 5
9. Start the game
10. The language should now be selectable in the options
