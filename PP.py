import glob
import sys
from keras.models import load_model
from keras.preprocessing import image
import numpy as np
from keras.models import Sequential

#print('Number of arguments: {}'.format(len(sys.argv)))
#print('Argument(s) passed: {}'.format(str(sys.argv)))

model = Sequential()

#print(str(sys.argv[1]))

model = Sequential()



lab = ["test"]
labels = ["2C", "2D", "2H", "2S",
          "3C", "3D", "3H", "3S",
          "4C", "4D", "4H", "4S",
          "5C", "5D", "5H", "5S",
          "6C", "6D", "6H", "6S",
          "7C", "7D", "7H", "7S",
          "8C", "8D", "8H", "8S",
          "9C", "9D", "9H", "9S",
          "10C", "10D", "10H", "10S",
          "AC", "AD", "AH", "AS",
          "JC", "JD", "JH", "JS",
          "KC", "KD", "KH", "KS",
          "QC", "QD", "QH", "QS"]

data = []
train_image = []

validate_image = []
validate_label = []

model = load_model('pokerTrainModel.h5')

#img = image.load_img('C:\\Users\mkosi\\PycharmProjects\\tensorEnv\\dataset\\4C\\test.jpg',target_size=(150,150,3))
#img = image.img_to_array(img)
img = np.load('4C.npy')
#np.save('4C', img)
classes = np.array(labels)
proba = model.predict(img.reshape(1,150,150,3))
top_3 = np.argsort(proba[0])[:-4:-1]
#print(classes[top_3[0]])

for i in range(3):
    print("{}".format(classes[top_3[i]])+" ({:.3})".format(proba[0][top_3[i]]))