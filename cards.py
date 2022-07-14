import glob
from keras.optimizers import SGD
import keras
from keras.applications import VGG16
from keras.models import Sequential
from keras.models import load_model
from keras.layers import Dense, Dropout, Flatten
from keras.layers import Conv2D, MaxPooling2D
from keras.layers import Reshape
from keras.utils import to_categorical
from keras.preprocessing import image
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.model_selection import train_test_split
from tqdm import tqdm
import tensorflow as tf
arr = []


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
"""
arr = []
for i in range(52):
    for j in range(0, 5):
        result = labels[i]
        hotOne = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
        hotOne[i] = 1
        arr.append(hotOne)

arr_validate = []
for i in range(52):
    for j in range(5, 10):
        result = labels[i]
        hotOne = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
        hotOne[i] = 1
        arr_validate.append(hotOne)

for i in range(52):
    for n in range(0, 5):
        img = image.load_img('dataset/{}/{}_v{}.jpg'.format(labels[i], labels[i], n), target_size=(150, 150, 3))
        img = image.img_to_array(img)
        img = img/255
        train_image.append(img)

for i in range(52):
    for n in range(5, 10):
        img = image.load_img('dataset/{}/{}_v{}.jpg'.format(labels[i], labels[i], n), target_size=(150, 150, 3))
        img = image.img_to_array(img)
        img = img/255
        validate_image.append(img)

X = np.array(train_image)
y = np.array(arr)

Xv = np.array(validate_image)
yv = np.array(arr_validate)

print(X.shape)
print(y.shape)

print(Xv.shape)
print(yv.shape)




model.add(Conv2D(filters=32, kernel_size=(5, 5), activation="relu", input_shape=(150,150,3)))
model.add(MaxPooling2D(pool_size=(2, 2)))
model.add(Dropout(0.25))

model.add(Conv2D(filters=64, kernel_size=(5, 5), activation='relu'))
model.add(MaxPooling2D(pool_size=(2, 2)))
model.add(Dropout(0.25))
model.add(Conv2D(filters=128, kernel_size=(5, 5), activation="relu"))
model.add(MaxPooling2D(pool_size=(2, 2)))
model.add(Dropout(0.25))

model.add(Conv2D(filters=128, kernel_size=(5, 5), activation='relu'))
model.add(MaxPooling2D(pool_size=(2, 2)))
model.add(Dropout(0.25))

model.add(Flatten())
model.add(Dense(128, activation='relu'))
model.add(Dropout(0.5))
model.add(Dense(64, activation='relu'))
model.add(Dropout(0.5))
model.add(Dense(52, activation='sigmoid'))

model.summary()
opt = tf.keras.optimizers.RMSprop()
model.compile(optimizer=opt, loss='categorical_crossentropy', metrics=['accuracy'])
model.fit(X, y, epochs=180, validation_data=(Xv, yv), batch_size=52)
"""
#model.save('TrainModel.h5')  # creates a HDF5 file 'my_model.h5'

model = load_model('TrainModel.h5')

img = image.load_img('dataset/10D/test.jpg',target_size=(150,150,3))
img = image.img_to_array(img)
img = img/255

classes = np.array(labels)
proba = model.predict(img.reshape(1,150,150,3))
top_3 = np.argsort(proba[0])[:-4:-1]
print(classes[top_3[0]])
#print("{}".format(classes[top_3[0]])+" ({:.3})".format(proba[0][top_3[0]]))
#for i in range(3):
 #   print("{}".format(classes[top_3[i]])+" ({:.3})".format(proba[0][top_3[i]]))
#plt.imshow(img)
#plt.show()

