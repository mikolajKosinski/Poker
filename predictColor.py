from azure.cognitiveservices.vision.customvision.prediction import CustomVisionPredictionClient
from msrest.authentication import ApiKeyCredentials
from matplotlib import pyplot as plt
from PIL import Image, ImageDraw, ImageFont
import numpy as np
from keras.models import Sequential
from keras.models import load_model
import tensorflow as tf
import sys
import random
import string
import base64
import numpy as np
from io import BytesIO
#from tqdm import tqdm
#import tensorflow as tf
import os
imgPath = sys.argv[1]
arr = []

#print(imgPath)
model = Sequential()

prediction_endpoint = "https://cardservice-prediction.cognitiveservices.azure.com/"
prediction_key = "1467c6b05fd347b7883575a7fd12d1cb"
content_type = "application/octet-stream"
project_id = "348905a0-326f-417e-adc2-ccdd74e841c9"
model_name = "Detect"

credentials = ApiKeyCredentials(in_headers={"Prediction-key": prediction_key})
predition_client = CustomVisionPredictionClient(endpoint=prediction_endpoint, credentials = credentials)

lab = ["test"]
labels = ["C", "D", "H", "S"]

data = []
train_image = []

validate_image = []
validate_label = []


#model = load_model(r'C:\Users\mkosi\PycharmProjects\pythonProject\\cATHV2.h5')
#image_file = r'C:\Users\mkosi\Documents\GitHub\Poker\RunPy\WpfClient\obj\Debug\net5.0-windows\C1.PNG'
#img = tf.keras.utils.load_img(imgPath,target_size=(30,30,3))
#img = tf.keras.utils.img_to_array(img)
#img = img/255
letters = string.ascii_lowercase
imgFromBytePath = ''.join(random.choice(letters) for i in range(10))
imgFromBytePath = imgFromBytePath+'.PNG'

decodeit = open(imgFromBytePath, 'wb')
decodeit.write(base64.b64decode((imgPath)))
decodeit.close()


with open(imgFromBytePath, mode="rb") as image_data:
    results = predition_client.classify_image(project_id, model_name, image_data)
#results = predition_client.classify_image(project_id, model_name, image)
os.remove(imgFromBytePath)
print(results.predictions[0].tag_name)

#############################################################################################
#results = predition_client.classify_image(project_id, model_name, img)
#for pred in results.predictions:
#    if pred.probability > 0.60:
#        print(pred.tag_name)

#classes = np.array(labels)
#proba = model.predict(img.reshape(1,30,30,3))
#top_3 = np.argsort(proba[0])[:-4:-1]

#score = tf.nn.softmax(proba[0])
#precentage = 100 * np.max(score)
#print(classes[top_3[0]])

#if precentage > 35:
    #print(classes[top_3[0]])
#else:
#    results = predition_client.classify_image(project_id, model_name, img)
#    for pred in results.predictions:
#        if pred.probability > 0.80:
#            print(pred.tag_name)



#print(precentage)
#print("{}".format(classes[top_3[0]])+" ({:.3})".format(proba[0][top_3[0]]))
#for i in range(3):
 #   print("{}".format(classes[top_3[i]])+" ({:.3})".format(proba[0][top_3[i]]))
#plt.imshow(img)
#plt.show()