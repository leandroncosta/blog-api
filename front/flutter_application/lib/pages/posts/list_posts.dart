import 'package:flutter/material.dart';
import 'package:flutter_application/components/card_post.dart';
import 'package:flutter_application/shared/controllers/post_controller.dart';

class ListPosts extends StatelessWidget {
   ListPosts({super.key});

  PostController postController = PostController();

  @override
  Widget build(BuildContext context) {
    print("tamanho atual da lista${postController.posts.length}");
    return ListView.builder(
      padding: const EdgeInsets.only(bottom: 50),
      itemCount: postController.posts.length,
      itemBuilder: ((context, index) {
        final post = postController.posts[index];
        //return Text(posts.length.toString());
        return CardPost(
            index:index,
            id: post.id,
            userId: post.userId,
            title: post.title,
            content: post.content,
            date: post.date);
      }),
    );
  }
}
