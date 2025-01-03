import 'package:flutter/material.dart';
import 'package:flutter_application/Providers/post_provider.dart';
import 'package:flutter_application/components/card_button.dart';
import 'package:flutter_application/shared/models/post_model.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class CardPost extends StatelessWidget {
  final int index;
  final Post post;
  CardPost({super.key, required this.index, required this.post});

  TextEditingController titleController = TextEditingController();
  TextEditingController contentController = TextEditingController();

  _showDeleteModal(context, id, index) {
    return showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
            title: Center(
              child: Text(
                style: const TextStyle(color: Colors.red),
                textAlign: TextAlign.center,
                "Tem certeza que deseja excluir o post de id: \n $id ?",
              ),
            ),
            content: const SizedBox(
              height: 2,
            ),
            actions: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  CardButton(
                      text: 'Cancelar',
                      onpressed: () => Navigator.pop(context)),
                  CardButton(
                      text: 'Deletar',
                      onpressed: () => {
                            context.read<PostProvider>().delete(id),
                            Navigator.of(context).pop()
                          }),
                  // ElevatedButton(
                  //   onPressed: () {
                  //     context.read<PostProvider>().delete(id);
                  //     Navigator.of(context).pop();
                  //   },
                  //   child: const Text(
                  //     "deletar",
                  //     style: TextStyle(color: Color.fromARGB(255, 255, 7, 40)),
                  //   ),
                  // ),
                ],
              )
            ]);
      },
    );
  }

  _showEditModal(context, id) {
    titleController.text = post.title;
    contentController.text = post.content;
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
                        label: Text(post.title),
                      ),
                      statesController: WidgetStatesController(),
                      controller: titleController,
                      onChanged: (value) {
                        titleController.text = value;
                        print("${titleController.value} test");
                      },
                    ),
                  ),
                  TextField(
                    decoration: InputDecoration(
                        border: const OutlineInputBorder(),
                        label: Text(post.content)),
                    controller: contentController,
                    onChanged: (value) {
                      contentController.text = value;
                      print("${contentController.value} test");
                    },
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
                  Post post = Post(
                      id: id,
                      userId: "",
                      title: titleController.text,
                      content: contentController.text,
                      date: "");
                  context.read<PostProvider>().update(post);
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
    var p = post;
    var date = DateTime.parse(p.date);
    var utc3Date = date.subtract(const Duration(hours: 3));
    var formatedDate = DateFormat("dd/MM/yyyy").add_jm().format(utc3Date);
    return Container(
      margin: const EdgeInsets.all(10),
      decoration: BoxDecoration(
          border: Border.all(
              color: const Color.fromARGB(255, 218, 225, 218), width: 3)),
      child: Column(
        children: [
          SizedBox(
           // height: 300,
            child: Column(
              spacing: 10,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                ListTile(
                  leading: const CircleAvatar(
                    child: Icon(Icons.photo_library_sharp),
                  ),
                  title: Text("Post id: ${post.id}"),
                ),
                const Divider(
                  thickness:2,
                  indent: 30,
                  endIndent: 30,),
                const SizedBox(
                  height: 20,
                ),
                Text(
                  "Post ${post.userId}",
                  style: const TextStyle(fontSize: 20),
                ),
                
                Text(
                  "Título - ${post.title}",
                  style: const TextStyle(fontSize: 20),
                ),
                Text(
                  "Conteúdo - ${post.content}",
                  style: const TextStyle(fontSize: 20),
                ),
                Text(
                  "Data de criação: $formatedDate",
                  style: const TextStyle(fontSize: 20),
                ),
                const SizedBox(
                  height: 10,
                ),
              ],
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(left: 10, right: 10),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: <Widget>[
                CardButton(
                    text: "Editar",
                    onpressed: () => _showEditModal(context, post.id)),
                CardButton(
                    text: "Deletar",
                    onpressed: () => _showDeleteModal(context, post.id, index)),
              ],
            ),
          ),
          const SizedBox(
            height: 20,
          )
        ],
      ),
    );
  }
}
