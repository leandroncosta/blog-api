import 'package:flutter/material.dart';
import 'package:flutter_application/shared/controllers/post_controller.dart';
import 'package:flutter_application/shared/models/post_model.dart';


class CardPost extends StatelessWidget {
  final int index;
  final String id;
  final String userId;
  final String title;
  final String content;
  final String date;
  CardPost(
      {super.key,
      required this.id,
      required this.userId,
      required this.title,
      required this.content,
      required this.date, 
      required  this.index});

  PostController postController = PostController();

  TextEditingController titleController = TextEditingController();

  TextEditingController contentController = TextEditingController();

  void _deletePost(id,index) {
    postController.delete(id);
  }

  void _editPost(id) {
    Post post = Post(
        id: id,
        userId: "",
        title: titleController.text,
        content: contentController.text,
        date: "");

    postController.update(post);
  }

  _showDeleteModal(context, id,index) {
    return showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
            title: Text(
              style: const TextStyle(
                color: Colors.red
                ),
              textAlign: TextAlign.center,
            "Tem certeza que deseja deletar o  post de id :$id ?", 
            ),
            content: const SizedBox(
              height: 50,
            ),
            actions: [
              ElevatedButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: const Text("cancelar"),
              ),
              ElevatedButton(
                onPressed: () {
                  _deletePost(id,index);
                  Navigator.of(context).pop();
                },
                child: const Text("deletar post"),
              ),
            ]);
      },
    );
  }

  _showEditModal(context, id) {
    return showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
            title: Text("Editar post de id $id?"),
            content: SizedBox(
              height: 150,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Form(
                    child: TextFormField(
                      validator: (value) {},
                      decoration: InputDecoration(
                          border: const OutlineInputBorder(),
                          hintText: 'test',
                          label: Text(title)),
                      statesController: WidgetStatesController(),
                      controller: titleController,

                      onChanged: (value) {
                        titleController.text = value;
                        print("${titleController.value} test");
                      },
                      //initialValue: "$id",
                    ),
                  ),
                  TextField(
                    decoration: InputDecoration(
                        border: const OutlineInputBorder(),
                        label: Text(content)),
                    controller: contentController,
                    onChanged: (value) {
                      contentController.text = value;
                      print("${contentController.value} test");
                    },
                    //initialValue: "$id",
                  )
                ],
              ),
            ),
            actions: [
              ElevatedButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: const Text("cancelar"),
              ),
              ElevatedButton(
                onPressed: () {
                  _editPost(id);
                  Navigator.of(context).pop();
                },
                child: const Text("salvar"),
              ),
            ]);
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
          border: Border.all(
              color: const Color.fromARGB(255, 218, 225, 218), width: 3)),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          SizedBox(
            height: 200,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text("Post id: $id",),
                const SizedBox(height: 20,
                ),
                Text("Post $userId",style: const TextStyle(fontSize: 20),),
                Text("Título - $title",style: const TextStyle(fontSize: 20),),
                Text("Conteúdo - $content",style: const TextStyle(fontSize: 20),),
                Text("Data:$date", style: const TextStyle(fontSize: 20),),
                const SizedBox(
                  height: 10,
                ),
              ],
            ),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: <Widget>[
              ElevatedButton(
                child: const Text('editar'),
                onPressed: () {
                  _showEditModal(context, id);
                },
              ),
              const SizedBox(width: 200),
              ElevatedButton(
                child: const Text('deletar'),
                onPressed: () {
                  _showDeleteModal(context,id,index);
                },
              ),
            ],
          ),
          const SizedBox(
            height: 20,
          )
        ],
      ),
    );
  }
}
