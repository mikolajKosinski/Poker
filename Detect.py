import string

from azure.cognitiveservices.vision.customvision.prediction import CustomVisionPredictionClient
from msrest.authentication import ApiKeyCredentials
from matplotlib import pyplot as plt
from PIL import Image, ImageDraw, ImageFont
import numpy as np
import sys
import os

def main():
    #name = sys.argv[1]
    prediction_endpoint = "https://cardservice-prediction.cognitiveservices.azure.com/"
    prediction_key = "1467c6b05fd347b7883575a7fd12d1cb"
    content_type = "application/octet-stream"
    project_id = "348905a0-326f-417e-adc2-ccdd74e841c9"
    model_name = "Detect"

    credentials = ApiKeyCredentials(in_headers={"Prediction-key": prediction_key})
    predition_client = CustomVisionPredictionClient(endpoint=prediction_endpoint, credentials = credentials)

    image_file = r'C:\Users\mkosi\Desktop\poker\detect\3_2.PNG'
    #print('Detecting in', image_file)
    image = Image.open(image_file)
    h, w, ch = np.array(image).shape

    with open(image_file, mode="rb") as image_data:
        results = predition_client.classify_image(project_id, model_name, image_data)

    fig = plt.figure(figsize=(8, 8))
    plt.axis('off')

    draw = ImageDraw.Draw(image)
    lineWidth = int(w/100)
    color = 'magenta'
    ind = 0
    lastWidth = 0
    for pred in results.predictions:
        if pred.probability > 0.98:
            left = pred.bounding_box.left * w
            top = pred.bounding_box.top * h
            height = pred.bounding_box.height * h
            width = pred.bounding_box.width * w
            points = ((left, top), (left + width, top), (left + width, height))
            # draw.line(points, fill=color, width=lineWidth)
            # print('----')
            # print(left)
            # print(width)
            # print(top)
            # print(height)
            # print(width)
            # print(pred.probability)
            outputCut = ''

            outputCutFigure = r'C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\\net5.0-windows\F%s.PNG'%ind
            outputCutColor = r'C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\\net5.0-windows\C%s.PNG' % ind
            #print(outputCut)

            crop_image_figure = image.crop((left, top, left + width, height + top))
            crop_image_figure.save(outputCutFigure)

            crop_image_color = image.crop((left, top + height, left + width, height + height + top))
            crop_image_color.save(outputCutColor)
            # print('saved'  +outputCut)
            ind = ind + 1

    #plt.imshow(image)
    #output = 'output.jpg'
    #fig.savefig(output)
    print(ind)


main()