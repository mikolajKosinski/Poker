import string

from azure.cognitiveservices.vision.customvision.prediction import CustomVisionPredictionClient
from msrest.authentication import ApiKeyCredentials
from matplotlib import pyplot as plt
from PIL import Image, ImageDraw, ImageFont
import numpy as np
import sys
import os
import sys
import random
import string
import base64
from keras.models import Sequential
from keras.models import load_model
import tensorflow as tf
import sys
import numpy as np
from numpy import asarray
imgPath = sys.argv[1]
arr = []


model = Sequential()



lab = ["test"]
labels = ["2", "3", "4", "5",
          "6", "7", "8", "9",
          "10", "j", "q", "k", "a"]

data = []
train_image = []

validate_image = []
validate_label = []

prediction_endpoint = "https://cardservice-prediction.cognitiveservices.azure.com/"
prediction_key = "1467c6b05fd347b7883575a7fd12d1cb"
content_type = "application/octet-stream"
project_id = "348905a0-326f-417e-adc2-ccdd74e841c9"
model_name = "Detect"

credentials = ApiKeyCredentials(in_headers={"Prediction-key": prediction_key})
predition_client = CustomVisionPredictionClient(endpoint=prediction_endpoint, credentials = credentials)

model = load_model(r'C:\Users\mkosi\PycharmProjects\pythonProject\\x86ATH.h5')
#image_file = r'C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\\net5.0-windows/Card%s_Figure.PNG'%cNumber
letters = string.ascii_lowercase
imgFromBytePath = ''.join(random.choice(letters) for i in range(10))
imgFromBytePath = imgFromBytePath+'.PNG'
decodeit = open(imgFromBytePath, 'wb')
decodeit.write(base64.b64decode((imgPath)))
decodeit.close()

with open(imgFromBytePath, mode="rb") as image_data:
    img = tf.keras.utils.load_img(imgFromBytePath,target_size=(30,30,3))
    img = tf.keras.utils.img_to_array(img)
    img = img/255

    classes = np.array(labels)
    proba = model.predict(img.reshape(1,30,30,3))
    top_3 = np.argsort(proba[0])[:-4:-1]
    result = classes[top_3[0]]

    if result == "4" or result == "a" or result == "5" or result == "3":
        #with open(imgPath, mode="rb") as image_data:
        results = predition_client.classify_image(project_id, model_name, image_data)

        for pred in results.predictions:
            if pred.probability > 0.90:
                result = pred.tag_name

    #if precentage < 17:
#        result = "a"
#    else:
#        result = "4"
#
#if result == "5":
#    score = tf.nn.softmax(proba[0])
#    precentage = 100 * np.max(score)
#    if precentage < 17:
#        result = "3"
#    else:
#        result = "5"

print(result)


#print("{}".format(classes[top_3[0]])+" ({:.3})".format(proba[0][top_3[0]]))
#for i in range(3):
 #   print("{}".format(classes[top_3[i]])+" ({:.3})".format(proba[0][top_3[i]]))
#plt.imshow(img)
#plt.show()