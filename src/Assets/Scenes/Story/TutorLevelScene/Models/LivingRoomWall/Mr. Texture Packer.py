import sys
import os

from PIL import Image, ImageOps

def main():
    directory = os.path.dirname(os.path.abspath(__file__))
    files = os.listdir(directory)

    extension = "png"    
    metalness_postfix, roughness_postfix = "Metalness", "Roughness"

    ending = f"{metalness_postfix}.{extension}"
    metalness_maps = filter(lambda name: name[-len(ending):-len(extension) - 1] == metalness_postfix, files)

    for filename in metalness_maps:
        try:
            metalness_map = Image.open(f"{directory}\\\\{filename}")

            roughness_map = Image.open(f"{directory}\\\\{str.replace(filename, metalness_postfix, roughness_postfix)}")
        except IOError:
            print(f"Couldn't open two maps for {filename}.")
            continue

        metalness_map = metalness_map.convert("RGBA")
        roughness_map = roughness_map.convert("L")

        as_smothness = True
        if (as_smothness):
            roughness_map = ImageOps.invert(roughness_map)

        metalness_map.putalpha(roughness_map)
        metalness_map.save(f"{directory}\\\\{filename}", extension)

    return 0

if __name__ == "__main__":
    sys.exit(main())